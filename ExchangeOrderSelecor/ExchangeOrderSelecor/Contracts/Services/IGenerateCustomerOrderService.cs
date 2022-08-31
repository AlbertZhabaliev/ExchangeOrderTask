using ExchangeOrderSelecor.Models.CustomerModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOrderSelecor.Contracts.Services
{
    public interface IGenerateCustomerOrderService
    {
        Task<CustomerOrder> PlaceBuyOrder(Customer customer, decimal btcToBuy);
        Task<CustomerOrder> PlaceSellOrder(Customer customer, decimal btcToSell);

    }
}
