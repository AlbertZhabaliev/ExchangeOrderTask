using ExchangeOrderSelecor.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOrderSelecor.Models.CustomerModel
{
    public class CustomerOrder
    {
        public int OrderId { get; set; }
        public int CustomerId{ get; set; }
        public DateTime OrderPlacementTime { get; set; }
        public OrderType OrderType{ get; set; }
        public Kind  OrderExecutaionType{ get; set; }

        //Simplified
        //public double PriceProBTC { get; set; }
        public decimal AmountOfBTC { get; set; }
        //public CurrencyType CurrencyType { get; set; }

    }
}
