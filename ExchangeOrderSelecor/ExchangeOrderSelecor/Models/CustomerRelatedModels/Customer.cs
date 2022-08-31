using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOrderSelecor.Models.CustomerModel
{
    public class Customer
    {
        //[Required]
        public int CustomerId { get; set; }
        //[Required]
        public string CustomerName { get; set; }
        //[Required]
        public CustomerWallet Wallet { get; set; }
        //[Required]
        public List<CustomerOrder> CustomerOrders { get; set; } = new List<CustomerOrder>();

    }
}
