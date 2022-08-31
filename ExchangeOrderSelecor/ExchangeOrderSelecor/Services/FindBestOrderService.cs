using ExchangeOrderSelecor.Contracts.Services;
using ExchangeOrderSelecor.Models;
using ExchangeOrderSelecor.Models.CustomerModel;
using ExchangeOrderSelecor.Models.OrderBookModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOrderSelecor.Services
{
    public class FindBestOrderService : IFindBestOrderService
    {
        public decimal PriceToPay { get; set; } = 0;
        public const int OneBitcoinInSatoshis = 100000000;

        Dictionary<SelectedOrder, decimal> bestOrderIds = new Dictionary<SelectedOrder, decimal>();//Id is missing, replacing it with a index and filename

        //TODO: Remove repetition, make Generic
        private SelectedOrders SelectorForAsks(CustomerOrder orders, Customer customer, IList<Asks> asks)
        {
            decimal btcToBuy = orders.AmountOfBTC;
            decimal fillOrder = 0;
            SelectedOrders selectedOrders = new();
            int i = 0;

            while (btcToBuy >= fillOrder & i < asks.Count /*&& customer.Wallet.EuroAvailable >= selectedOrders.PriceToPay*/)
            {
                SelectedOrder selected = new();
                fillOrder = fillOrder + asks[i].Order.Amount;
                selectedOrders.PriceToPay = selectedOrders.PriceToPay + asks[i].Order.Price * asks[i].Order.Amount;
                selected.PriceProBtc = asks[i].Order.Price;
                selected.Id = i;
                selectedOrders.ordersSelected.Add(selected);
                i++;
            }
            if (customer.Wallet.EuroAvailable <= selectedOrders.PriceToPay || i == asks.Count)
            {
                selectedOrders.IsExcecutable = false;
                return selectedOrders;
            }
            decimal tmpDiff = fillOrder - btcToBuy;
            fillOrder = fillOrder - tmpDiff;

            selectedOrders.PriceToPay = selectedOrders.PriceToPay - (asks[i].Order.Price * tmpDiff);
 
            selectedOrders.IsExcecutable = true;

            return selectedOrders;
        }
        private SelectedOrders SelectorForBids(CustomerOrder orders, Customer customer, IList<Bids> bids)
        {
            decimal btcToSell = orders.AmountOfBTC;
            decimal fillOrder = 0;
            SelectedOrders selectedOrders = new();
            int i = 0;

            while (btcToSell >= fillOrder && bids.Count > i/*&& customer.Wallet.EuroAvailable >= selectedOrders.PriceToPay*/)
            {
                SelectedOrder selected = new();
                fillOrder = fillOrder + bids[i].Order.Amount;
                selectedOrders.PriceToGet = selectedOrders.PriceToGet + bids[i].Order.Price * bids[i].Order.Amount;
                selected.PriceProBtc = bids[i].Order.Price;
                selected.Id = i;
                selectedOrders.ordersSelected.Add(selected);
                i++;
            }

            decimal tmpDiff = fillOrder - btcToSell;
            fillOrder = fillOrder - tmpDiff;

            selectedOrders.PriceToGet = selectedOrders.PriceToGet - (bids[i].Order.Price * tmpDiff);
            
            selectedOrders.IsExcecutable = true;
            return selectedOrders;
        }

        public async Task<SelectedOrders> GetBestOrdersToSell2Async(CustomerOrder order, Customer customer)
        {
            var res = await GetAllOrdersFromExchanges();
            List<SelectedOrders> selectedOrders = new();
            ParallelOptions parallelOptions = new()
            {
                MaxDegreeOfParallelism = 1
            };

            Parallel.ForEach(res, parallelOptions, orderBook =>
            {
                SelectedOrders selectedOrderInCurrentBids = new();
                selectedOrderInCurrentBids = SelectorForBids(order, customer, orderBook.Value.Bids);
                selectedOrderInCurrentBids.FileJsonName = orderBook.Key;
                selectedOrders.Add(selectedOrderInCurrentBids);
            });
            
            var bestOrder = selectedOrders./*Where(o=> o.IsExcecutable)*/OrderBy(p => p.PriceToGet).LastOrDefault();

            return bestOrder;
        }

        public async Task<SelectedOrders> GetBestOrdersToBuy2Async(CustomerOrder orders, Customer customer)
        {
        var res = await GetAllOrdersFromExchanges();
        List<SelectedOrders> selectedOrders = new();
        ParallelOptions parallelOptions = new()
        {
            MaxDegreeOfParallelism = 1
        };

        Parallel.ForEach(res, parallelOptions, orderBook =>
        {
            SelectedOrders selectedOrderInCurrentAsk = new();
            selectedOrderInCurrentAsk = SelectorForAsks(orders, customer, orderBook.Value.Asks);
            selectedOrderInCurrentAsk.FileJsonName = orderBook.Key;
            selectedOrders.Add(selectedOrderInCurrentAsk);
        });

        var bestOrder = selectedOrders./*Where(o=> o.IsExcecutable)*/OrderBy(p => p.PriceToPay).FirstOrDefault();

        return bestOrder;

        }
        
        public async Task<Dictionary<SelectedOrder, decimal>> GetOrdersToBuy(CustomerOrder orders, Customer customer)
        {
            //after finishing this function I saw that I got the buy sell process wrong.

            //Analyzing the JsonFile showed that it is sorted, so possible sorting will be skipped
            List<Asks> askas = await GetAsksSingleExchange();
            decimal btcToBuy = orders.AmountOfBTC;
            decimal fillOrder = 0;
            //decimal priceToPay = 0;
            int i = 0;

            while ((btcToBuy != fillOrder)) //fill the order
            {

                if (btcToBuy < askas[i].Order.Amount)
                {//this single position is bigger then the Order needs to be filled, so skip
                    i++;
                    continue;
                }

                //try to fill the order
                decimal tmpDiff = btcToBuy - fillOrder;
                if (tmpDiff < askas[i].Order.Amount) //Adding this btc Amount will exceed the BuyAmount
                {
                    i++;
                    continue;
                }

                fillOrder = fillOrder + askas[i].Order.Amount;

                SelectedOrder selectedOrder = new();
                selectedOrder.Id = i;
                selectedOrder.PriceProBtc = askas[i].Order.Price;
                PriceToPay = PriceToPay + askas[i].Order.Price;

                bestOrderIds.Add(selectedOrder, askas[i].Order.Amount);
                if (fillOrder == btcToBuy)
                {
                    return bestOrderIds;
                }
                i++;
            }

            return bestOrderIds;
        }

        public async Task<SelectedOrders> GetOrdersToBuy2Async(CustomerOrder orders, Customer customer)
        {
            List<Asks> asks = await GetAsksSingleExchange();
            SelectedOrders selectedOrders = SelectorForAsks(orders, customer, asks);
            return selectedOrders;
        }
        
        
        private async Task<List<Asks>> GetAsksSingleExchange()
        {
            GetSingleOrderBookService getSingleOrderBookService = new();
            //Assuming the Order Book is already sorted
            var res = await getSingleOrderBookService.ReadFromLocalStorageJsonOrderBook();
            return res.Asks.ToList();
        }

        private async Task<Dictionary<string, Orders>> GetAllOrdersFromExchanges()
        {
            GetSingleOrderBookService getSingleOrderBookService = new();
            //Assuming the Order Book is already sorted
            var res = await getSingleOrderBookService.ReadAllFilesFromLocalJsonOrderBook();
            return res;
        }
    }
}
