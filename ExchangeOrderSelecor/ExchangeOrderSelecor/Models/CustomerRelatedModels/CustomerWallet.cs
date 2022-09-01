using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOrderSelecor.Models.CustomerModel
{
    public class CustomerWallet
    {
        //Simplified Wallet Model
        public int CustomerId { get; set; }
        public decimal EuroAvailable { get; set; }
        public decimal BTCAvailable { get; set; }
    }

}

