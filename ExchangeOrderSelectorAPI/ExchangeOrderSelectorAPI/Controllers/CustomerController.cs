using ExchangeOrderSelecor.Models.CustomerModel;
using ExchangeOrderSelectorAPI.Contracts;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExchangeOrderSelectorAPI.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
           
        [HttpGet("{id}")]
        public async Task<Customer> Get(int id)
        {
            return await _customerRepository.GetCustomerAsync(id);
        }
    }
}
