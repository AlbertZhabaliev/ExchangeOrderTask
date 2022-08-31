using ExchangeOrderSelecor.Contracts.Services;
using ExchangeOrderSelecor.Models.CustomerModel;
using ExchangeOrderSelectorAPI.Contracts;

namespace ExchangeOrderSelectorAPI.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        public Customer CurrentCustomer { get; private set; }
        private readonly IGenerateSampleCustomerService GenereateSampleCustomer;

        public CustomerRepository(IGenerateSampleCustomerService genereateSampleCustomer)
        {
            GenereateSampleCustomer = genereateSampleCustomer;
        }


        public async Task<Customer> GetCustomer()
        {
            CurrentCustomer = await GenereateSampleCustomer.GetSampleCustomer();
            return CurrentCustomer;
        }
    }
}
