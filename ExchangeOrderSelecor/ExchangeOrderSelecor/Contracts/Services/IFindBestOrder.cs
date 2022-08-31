using ExchangeOrderSelecor.Models;
using ExchangeOrderSelecor.Models.CustomerModel;
using ExchangeOrderSelecor.Models.OrderBookModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOrderSelecor.Contracts.Services
{
    public interface IFindBestOrderService
    {
        Task<SelectedOrders> GetBestOrdersToBuy2Async(CustomerOrder orders, Customer customer);
        Task<SelectedOrders> GetBestOrdersToSell2Async(CustomerOrder orders, Customer customer);


        #region Depricated
        Task<Dictionary<SelectedOrder, decimal>> GetOrdersToBuy(CustomerOrder orders, Customer customer);
        Task<SelectedOrders> GetOrdersToBuy2Async(CustomerOrder orders, Customer customer);
        #endregion
    }
}
