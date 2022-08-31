using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOrderSelecor.Models.OrderBookModel
{ 
    public class Bids
    {
        [JsonProperty("Order")]
        public Order Order { get; set; }
    }
}
