using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOrderSelecor.Models.OrderBookModel
{
    public class OrderSelection
    {
        public string ExchangeFileName { get; set; } = string.Empty;
        public decimal PurchasePrice { get; set; } = 0;
        public decimal SalesPrice { get; set; } = 0;
        public bool IsExcecutable { get; set; } = false;
        public List<SelectedOrder> SelectedOrders { get; set; } = new List<SelectedOrder>();
    }
}
