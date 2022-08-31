using ExchangeOrderSelecor.Contracts.Services;
using ExchangeOrderSelecor.Helpers;
using ExchangeOrderSelecor.Models;
using ExchangeOrderSelecor.Models.CustomerModel;
using ExchangeOrderSelectorAPI.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExchangeOrderSelectorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceOrderController : ControllerBase
    {
        private readonly ICustomerRepository SampleCustomer;
        private readonly IGenerateSampleCustomerService GenereateSampleCustomer;
        private readonly IFindBestOrderService FindBestOrderService;


        CultureInfo provider = new("us-US");

        public PlaceOrderController(ICustomerRepository getSampleCustomer,
            IGenerateSampleCustomerService genereateSampleCustomer,
            IFindBestOrderService findBestOrderService
            )
        {
            SampleCustomer = getSampleCustomer;
            GenereateSampleCustomer = genereateSampleCustomer;
            FindBestOrderService = findBestOrderService;

        }


        //// GET: api/<PlaceOrderController>
        //[HttpGet]
        //public async Task<ActionResult<Customer>> Get()
        //{

        //}

        // GET api/<PlaceOrderController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{

        //    return "value";
        //}

        // POST api/<PlaceOrderController>Hope 
        [HttpPost]
        public async Task<ActionResult<string>> Post([FromBody] CustomerOrder customerOrder)
        {
            if (SampleCustomer.CurrentCustomer == null)
            {
                return NotFound("Customer not created yet, please call first the Get Customer Endpoint");
            }

            try
            {
                decimal btcToProcess = Decimal.Parse(customerOrder.AmountOfBTC.ToString(), provider);

                switch (customerOrder.OrderType)
                {
                    case ExchangeOrderSelecor.Models.Enums.OrderType.Buy:
                        var buyRes = await FindBestOrderService.GetBestOrdersToBuy2Async(customerOrder, SampleCustomer.CurrentCustomer);
                        if (buyRes != null && buyRes.IsExcecutable == false)
                        {
                            return BadRequest("Order cann't be executed, please check your balance");
                        }
                        string buyResp = await Json.StringifyAsync(buyRes);
                        return buyResp;
                        

                    case ExchangeOrderSelecor.Models.Enums.OrderType.Sell:
                        if (SampleCustomer.CurrentCustomer.Wallet.BTCAvailable < customerOrder.AmountOfBTC)
                        {
                            return BadRequest("The Sell amount exceeds you BTC balance");

                        }
                        var sellRes = await FindBestOrderService.GetBestOrdersToSell2Async(customerOrder, SampleCustomer.CurrentCustomer);
                        string sellResp = await Json.StringifyAsync(sellRes);
                        return sellResp;
                        
                    default:
                        return BadRequest("Somthing went wrong");
                        
                }
            }
            catch (Exception)
            {

                return ValidationProblem("Not a Valid BTC Format");
            }
        }



        //// PUT api/<PlaceOrderController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<PlaceOrderController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
