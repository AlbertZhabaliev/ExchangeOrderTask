using ExchangeOrderSelecor.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOrderSelecor.Models.OrderBookModel
{
    public class Order
    {
        [JsonProperty("Id")]
        public int? Id { get; set; }
        
        [JsonProperty("Time")]
        public DateTime? Time { get; set; }
        
        [JsonProperty("Type")]
        public OrderType? Type { get; set ; }
        
        [JsonProperty("Kind")]
        public Kind? Kind { get; set; }

        [JsonProperty("Amount")]
        public decimal Amount { get; set;}
        
        [JsonProperty("Price")]
        public decimal Price { get; set;}

        //public Status OrderStatus{ get; set; }
    }
}
