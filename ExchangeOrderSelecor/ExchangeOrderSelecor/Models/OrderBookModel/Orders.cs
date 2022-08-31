using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOrderSelecor.Models.OrderBookModel
{
    //Created with Helf of https://app.quicktype.io/
    public class Orders
    {
        [JsonProperty("AcqTime")]
        public string AcqTime { get; set; }

        [JsonProperty("Bids")]
        public IList<Bids> Bids { get; set; }

        [JsonProperty("Asks")]
        public IList<Asks> Asks { get; set; }

    }
}
