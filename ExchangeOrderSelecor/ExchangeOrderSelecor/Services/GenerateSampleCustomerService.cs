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
        public Task<Customer> GetSampleCustomer()
        {
            Customer sampleCustomer = new Customer()
            {
                CustomerId = 1,
                CustomerName = "Sample Customer",
                CustomerOrders = new(),
                Wallet = new()
                {
                    CustomerId = 1,
                    BTCAvailable = GetRandomBTCAmount(),
                    EuroAvailable = GetRandomEuroAmount(),
                }
            };

            return Task.FromResult(sampleCustomer);
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
    }
}
