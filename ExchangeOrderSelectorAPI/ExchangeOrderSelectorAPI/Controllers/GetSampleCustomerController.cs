using ExchangeOrderSelecor.Models.CustomerModel;
using ExchangeOrderSelectorAPI.Contracts;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExchangeOrderSelectorAPI.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class GetSampleCustomerController : ControllerBase
    {
        private readonly ICustomerRepository GetSampleCustomer;

        public GetSampleCustomerController(ICustomerRepository getSampleCustomer)
        {
            GetSampleCustomer = getSampleCustomer;
        }

        // GET: api/<GetSampleCustomerController>
        [HttpGet]
        public async Task<Customer> Get()
        {
            return await GetSampleCustomer.GetCustomer();
        }

        //// GET api/<GetSampleCustomerController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<GetSampleCustomerController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<GetSampleCustomerController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<GetSampleCustomerController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
