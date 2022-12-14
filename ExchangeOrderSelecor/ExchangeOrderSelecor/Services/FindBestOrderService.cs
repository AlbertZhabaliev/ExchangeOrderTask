using ExchangeOrderSelecor.Contracts.Services;
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

        //TODO: Remove repetition, make Generic
       


        private OrderSelection SelectorForAsks(CustomerOrder orders, Customer customer, IList<Asks> asks)
        {
            decimal btcToBuy = orders.AmountOfBTC;
            decimal fillOrder = 0;
            OrderSelection selectedOrders = new();
            int i = 0;

            while (btcToBuy >= fillOrder & i < asks.Count /*&& customer.Wallet.EuroAvailable >= selectedOrders.PriceToPay*/)
            {
                SelectedOrder selected = new();
                fillOrder = fillOrder + asks[i].Order.Amount;
                selectedOrders.PurchasePrice = selectedOrders.PurchasePrice + asks[i].Order.Price * asks[i].Order.Amount;
                selected.PriceProBtc = asks[i].Order.Price;
                selected.Id = i;
                selectedOrders.SelectedOrders.Add(selected);
                i++;
            }
            if (customer.Wallet.EuroAvailable <= selectedOrders.PurchasePrice || i == asks.Count)
            {
                selectedOrders.IsExcecutable = false;
                return selectedOrders;
            }
            decimal tmpDiff = fillOrder - btcToBuy;
            fillOrder = fillOrder - tmpDiff;

            selectedOrders.PurchasePrice = selectedOrders.PurchasePrice - (asks[i].Order.Price * tmpDiff);
 
            selectedOrders.IsExcecutable = true;

            return selectedOrders;
        }
        private OrderSelection SelectorForBids(CustomerOrder orders, Customer customer, IList<Bids> bids)
        {
            decimal btcToSell = orders.AmountOfBTC;
            decimal fillOrder = 0;
            OrderSelection selectedOrders = new();
            int i = 0;

            while (btcToSell >= fillOrder && bids.Count > i/*&& customer.Wallet.EuroAvailable >= selectedOrders.PriceToPay*/)
            {
                SelectedOrder selected = new();
                fillOrder = fillOrder + bids[i].Order.Amount;
                selectedOrders.SalesPrice = selectedOrders.SalesPrice + bids[i].Order.Price * bids[i].Order.Amount;
                selected.PriceProBtc = bids[i].Order.Price;
                selected.Id = i;
                selectedOrders.SelectedOrders.Add(selected);
                i++;
            }

            decimal tmpDiff = fillOrder - btcToSell;
            fillOrder = fillOrder - tmpDiff;

            selectedOrders.SalesPrice = selectedOrders.SalesPrice - (bids[i].Order.Price * tmpDiff);
            
            selectedOrders.IsExcecutable = true;
            return selectedOrders;
        }

        public async Task<OrderSelection> GetBestOrdersToSell2Async(CustomerOrder order, Customer customer)
        {
            var res = await GetAllOrdersFromExchanges();
            List<OrderSelection> selectedOrders = new();
            ParallelOptions parallelOptions = new()
            {
                MaxDegreeOfParallelism = 1
            };

            Parallel.ForEach(res, parallelOptions, orderBook =>
            {
                OrderSelection selectedOrderInCurrentBids = new();
                selectedOrderInCurrentBids = SelectorForBids(order, customer, orderBook.Value.Bids);
                selectedOrderInCurrentBids.ExchangeFileName = orderBook.Key;
                selectedOrders.Add(selectedOrderInCurrentBids);
            });
            
            var bestOrder = selectedOrders./*Where(o=> o.IsExcecutable)*/OrderBy(p => p.SalesPrice).LastOrDefault();

            return bestOrder;
        }

        #region deprecated, this Functions were just for the first testings
        //Id is missing, replacing it with the index and filename
        Dictionary<SelectedOrder, decimal> bestOrderIds = new Dictionary<SelectedOrder, decimal>();
        public async Task<OrderSelection> GetBestOrdersToBuy2Async(CustomerOrder orders, Customer customer)
        {
        var res = await GetAllOrdersFromExchanges();
        List<OrderSelection> selectedOrders = new();
        ParallelOptions parallelOptions = new()
        {
            MaxDegreeOfParallelism = 1
        };

        Parallel.ForEach(res, parallelOptions, orderBook =>
        {
            OrderSelection selectedOrderInCurrentAsk = new();
            selectedOrderInCurrentAsk = SelectorForAsks(orders, customer, orderBook.Value.Asks);
            selectedOrderInCurrentAsk.ExchangeFileName = orderBook.Key;
            selectedOrders.Add(selectedOrderInCurrentAsk);
        });

        var bestOrder = selectedOrders./*Where(o=> o.IsExcecutable)*/OrderBy(p => p.PurchasePrice).FirstOrDefault();

        return bestOrder;

        }
        private async Task<Dictionary<string, Orders>> GetAllOrdersFromExchanges()
        {
            GetSingleOrderBookService getSingleOrderBookService = new();
            //Assuming the Order Book is already sorted
            var res = await getSingleOrderBookService.ReadAllFilesFromLocalJsonOrderBook();
            return res;
        }
        

        public async Task<Dictionary<SelectedOrder, decimal>> GetOrdersToBuy(CustomerOrder orders, Customer customer)
        {
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

        public async Task<OrderSelection> GetOrdersToBuy2Async(CustomerOrder orders, Customer customer)
        {
            List<Asks> asks = await GetAsksSingleExchange();
            OrderSelection selectedOrders = SelectorForAsks(orders, customer, asks);
            return selectedOrders;
        }
        
        private async Task<List<Asks>> GetAsksSingleExchange()
        {
            GetSingleOrderBookService getSingleOrderBookService = new();
            //Assuming the Order Book is already sorted
            var res = await getSingleOrderBookService.ReadFromLocalStorageJsonOrderBook();
            return res.Asks.ToList();
        }
        #endregion
    }
}
