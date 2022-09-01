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
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PlaceOrderController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IFindBestOrderService _findBestOrderService;

        public PlaceOrderController(ICustomerRepository getSampleCustomer,
            IFindBestOrderService findBestOrderService
            )
        {
            _customerRepository = getSampleCustomer;
            _findBestOrderService = findBestOrderService;
        }


        // POST api/<PlaceOrderController>Hope 
        [HttpPost]
        public async Task<ActionResult<string>> Post([FromBody] CustomerOrder customerOrder)
        {
            if (_customerRepository.CurrentCustomer == null)
            {
                return NotFound("Customer not created yet, please call first the Get Customer Endpoint");
            }

            try
            {
                decimal btcToProcess = Decimal.Parse(customerOrder.AmountOfBTC.ToString());

                switch (customerOrder.OrderType)
                {
                    case ExchangeOrderSelecor.Models.Enums.OrderType.Buy:

                        return await SelectBuyOrder(customerOrder);

                    case ExchangeOrderSelecor.Models.Enums.OrderType.Sell:

                        return await SellectSellOrder(customerOrder);

                    default:
                        return BadRequest("Somthing went wrong");
                }
            }
            catch (Exception)
            {
                return ValidationProblem("Not a Valid BTC Format");
            }
        }

        private async Task<ActionResult<string>> SellectSellOrder(CustomerOrder customerOrder)
        {
            if (_customerRepository.CurrentCustomer.Wallet.BTCAvailable < customerOrder.AmountOfBTC)
            {
                return BadRequest("The Sell amount exceeds you BTC balance");

            }
            var sellRes = await _findBestOrderService.GetBestOrdersToSell2Async(customerOrder, _customerRepository.CurrentCustomer);
            string sellResp = await Json.StringifyAsync(sellRes);
            return sellResp;
        }

        private async Task<ActionResult<string>> SelectBuyOrder(CustomerOrder customerOrder)
        {
            var buyRes = await _findBestOrderService.GetBestOrdersToBuy2Async(customerOrder, _customerRepository.CurrentCustomer);
            if (buyRes != null && buyRes.IsExcecutable == false)
            {
                return BadRequest("Order cann't be executed, please check your balance");
            }
            string buyResp = await Json.StringifyAsync(buyRes);
            return buyResp;
        }
    }
}
