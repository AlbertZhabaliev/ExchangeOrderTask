using ExchangeOrderSelecor.Contracts.Services;
using ExchangeOrderSelecor.Models.CustomerModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOrderSelecor.Services
{
    public class GenerateSampleCustomerService : IGenerateSampleCustomerService
    {
        //this property is just for the simulation normally cutomer is grabed through the reposotory after LogIn
        public Customer Customer { get; private set; }
        public Task<Customer> GetSampleCustomer(int i)
        {
            Customer sampleCustomer = new Customer()
            {
                CustomerId = i,
                CustomerName = "Sample Customer",
                CustomerOrders = new(),
                Wallet = new()
                {
                    CustomerId = i,
                    BTCAvailable = GetRandomBTCAmount(),
                    EuroAvailable = GetRandomEuroAmount(),
                }
            };
            if (Customer == null)
            {
                Customer = sampleCustomer;
                return Task.FromResult(Customer);
            }
            if (Customer.CustomerId == i)
            {
                return Task.FromResult(Customer);
            }
           
            return Task.FromResult(dummyCustomer);
        }

        private decimal GetRandomEuroAmount()
        {
            Random random = new Random();

            int euroRandom = random.Next(10000, 50000);
            return (decimal)euroRandom;
        }

        private long GetRandomBTCAmount()
        {
            Random random = new Random();

            int btcRandom = random.Next(2, 12);
            return (long)btcRandom;
        }
        Customer dummyCustomer = new Customer()
        {
            CustomerId = 0,
            CustomerName = "Dummy Customer",
            CustomerOrders = new(),
            Wallet = new()
            {
                CustomerId = 0,
                BTCAvailable = 0,
                EuroAvailable = 0,
            }
        };

    }
}
