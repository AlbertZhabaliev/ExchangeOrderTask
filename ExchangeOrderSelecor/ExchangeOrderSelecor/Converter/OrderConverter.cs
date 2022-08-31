using ExchangeOrderSelecor.Models.OrderBookModel;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOrderSelecor.Converter
{
    public class OrderConverter : CustomCreationConverter<Order>
    {
        public override Order Create(Type objectType)
        {
            return new Order();
        }
    }
}
