using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOrderSelecor.Models.OrderBookModel
{
    public class Asks
    {
        //[JsonProperty("Order")]
        //public ICollection<Order> Orders { get; set; }

        [JsonProperty("Order")]
        public Order Order { get; set; }
    }
}
