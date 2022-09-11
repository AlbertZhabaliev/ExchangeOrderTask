using ExchangeOrderSelecor.Contracts.Services;
using ExchangeOrderSelecor.Models.CustomerModel;
using ExchangeOrderSelectorAPI.Contracts;

namespace ExchangeOrderSelectorAPI.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IGenerateSampleCustomerService GenereateSampleCustomer;

        public CustomerRepository(IGenerateSampleCustomerService genereateSampleCustomer)
        {
            GenereateSampleCustomer = genereateSampleCustomer;
        }

        public async Task<Customer> GetCustomerAsync(int id)
        {
            return await GenereateSampleCustomer.GetSampleCustomer(id);
        }
    }
}
