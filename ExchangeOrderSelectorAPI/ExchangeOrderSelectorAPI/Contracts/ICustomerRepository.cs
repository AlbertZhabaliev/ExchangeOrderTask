using ExchangeOrderSelecor.Models.CustomerModel;

namespace ExchangeOrderSelectorAPI.Contracts
{
    public interface ICustomerRepository
    {
        //public Customer CurrentCustomer { get;}
        Task<Customer> GetCustomerAsync(int id);
    }
}
