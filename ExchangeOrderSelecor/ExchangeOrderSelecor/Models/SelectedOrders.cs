using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOrderSelecor.Models
{
    public class SelectedOrders
    {
        public string FileJsonName { get; set; } = string.Empty;
        public decimal PriceToPay { get; set; } = 0;
        public decimal PriceToGet { get; set; } = 0;

        public bool IsExcecutable = false;
        
        public List<SelectedOrder> ordersSelected = new List<SelectedOrder>();
    }

    public class SelectedOrder
    {
        public int Id { get; set; } = 0;
        public decimal PriceProBtc { get; set; } = 0;
        
    }
}
