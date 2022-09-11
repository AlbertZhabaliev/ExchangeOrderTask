using ExchangeOrderSelecor.Models.CustomerModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOrderSelecor.Contracts.Services
{
    public interface IGenerateSampleCustomerService
    {
        Task<Customer> GetSampleCustomer(int id);
    }
}
