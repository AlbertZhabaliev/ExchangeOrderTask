using ExchangeOrderSelecor.Models.OrderBookModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOrderSelecor.Contracts.Services
{
    public interface IGetSingleOrderBookService
    {
        //public IFileService FileService { get; }
        Task<List<Order>> GetSingleOrderBook();
        Task<Orders> ReadFromLocalStorageJsonOrderBook();
    }
}
