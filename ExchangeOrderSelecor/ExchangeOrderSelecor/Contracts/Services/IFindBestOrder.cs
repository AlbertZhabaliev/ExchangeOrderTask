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
        Task<OrderSelection> GetBestOrdersToBuy2Async(CustomerOrder orders, Customer customer);
        Task<OrderSelection> GetBestOrdersToSell2Async(CustomerOrder orders, Customer customer);

        #region deprecated, this Functions were just for the first testings
        Task<Dictionary<SelectedOrder, decimal>> GetOrdersToBuy(CustomerOrder orders, Customer customer);
        Task<OrderSelection> GetOrdersToBuy2Async(CustomerOrder orders, Customer customer);
        #endregion
    }
}
