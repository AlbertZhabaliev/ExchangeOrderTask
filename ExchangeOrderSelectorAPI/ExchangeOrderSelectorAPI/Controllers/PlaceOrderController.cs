using ExchangeOrderSelecor.Contracts.Services;
using ExchangeOrderSelecor.Helpers;
using ExchangeOrderSelecor.Models.CustomerModel;
using ExchangeOrderSelecor.Models.Enums;
using ExchangeOrderSelecor.Models.OrderBookModel;
using ExchangeOrderSelectorAPI.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
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


        #region Classes
        public class CustomerOrderDto
        {
            public int OrderId { get; set; }

            [Range(0, int.MaxValue)]
            public int CustomerId { get; set; }
            public DateTime OrderPlacementTime { get; set; }
            public OrderType OrderType { get; set; }
            public Kind OrderExecutaionType { get; set; }

            [Range(0.0, (double)decimal.MaxValue)]
            public decimal AmountOfBTC { get; set; }

        }
        #endregion


        public PlaceOrderController(ICustomerRepository customerRepository,
            IFindBestOrderService findBestOrderService
            )
        {
            _customerRepository = customerRepository;
            _findBestOrderService = findBestOrderService;
        }

        
        [HttpPost]
        public async Task<ActionResult<OrderSelection>> Post([FromBody] CustomerOrder customerOrder)
        {
            var actionResult = ValidateModel(customerOrder);
            if (actionResult == NoContent())
                return actionResult;

            var customer = await _customerRepository.GetCustomerAsync(customerOrder.CustomerId);
            if (customer.CustomerId == 0)
            {
                return NotFound("Customer not found.");
            }

            try
            {

                switch (customerOrder.OrderType)
                {
                    case OrderType.Buy:

                        return await SelectBuyOrder(customerOrder, customer);

                    case OrderType.Sell:

                        return await SellectSellOrder(customerOrder, customer);

                    default:
                        return BadRequest("Somthing went wrong");
                }
            }
            catch (Exception)
            {
                return ValidationProblem("Not a Valid BTC Format");
            }
        }


        private ActionResult ValidateModel(CustomerOrder customerOrder)
        {
            if (customerOrder.CustomerId <= 0 )
                return BadRequest($"{nameof(customerOrder.CustomerId)} must be greater 0");

            if (customerOrder.AmountOfBTC <= 0)
                return BadRequest($"{nameof(customerOrder.AmountOfBTC)} must be greater 0");

            return NoContent();

            //ModelState.AddModelError("AmountOfBTC", "Must be greater than 0");
            //if (this.ModelState.IsValid == false)
            //    return BadRequest(ModelState);
        }

        private async Task<ActionResult<OrderSelection>> SellectSellOrder(CustomerOrder customerOrder, Customer customer)
        {
            if (customer.Wallet.BTCAvailable < customerOrder.AmountOfBTC)
                return BadRequest("The Sell amount exceeds you BTC balance");

            var sellRes = await _findBestOrderService.GetBestOrdersToSell2Async(customerOrder, customer);

            return Ok(sellRes);
        }

        private async Task<ActionResult<OrderSelection>> SelectBuyOrder(CustomerOrder customerOrder, Customer customer)
        {
            var buyRes = await _findBestOrderService.GetBestOrdersToBuy2Async(customerOrder, customer);
            if (buyRes != null && buyRes.IsExcecutable == false)
                return BadRequest("Order cann't be executed, please check your balance");

            return Ok(buyRes);
        }
    }
}
