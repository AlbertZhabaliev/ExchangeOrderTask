using ExchangeOrderSelecor.Contracts.Services;
using ExchangeOrderSelecor.Models.CustomerModel;
using ExchangeOrderSelecor.Models.Enums;

namespace ExchangeOrderSelecor.Services
{
    public class GenerateCustomerOrderService : IGenerateCustomerOrderService
    {

        public GenerateCustomerOrderService()
        {

        }
        public Task<CustomerOrder> PlaceBuyOrder(Customer customer, decimal btcToBuy)
        {
           
            CustomerOrder tmp = new()
            {
                CustomerId = customer.CustomerId,
                AmountOfBTC = btcToBuy,
                OrderId = 1,
                OrderExecutaionType = Kind.Market,
                OrderType = OrderType.Buy,
                OrderPlacementTime = DateTime.Now,  
            };

            return Task.FromResult(tmp);
        }
        public Task<CustomerOrder> PlaceSellOrder(Customer customer, decimal btcToSell)
        {
            CustomerOrder tmp = new()
            {
                CustomerId = customer.CustomerId,
                AmountOfBTC = btcToSell,
                OrderId = 2,
                OrderExecutaionType = Kind.Market,
                OrderType = OrderType.Sell,
                OrderPlacementTime = DateTime.Now,
            };

            return Task.FromResult(tmp);
        }
    }
}
