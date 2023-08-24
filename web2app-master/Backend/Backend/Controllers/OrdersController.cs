using Backend.DTO;
using Backend.Interfaces;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderServ _orderService;

        public OrdersController(IOrderServ orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("create")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "Customer")]
        public async Task<IActionResult> Create(OrderCreateDto newOrder)
        {
            var result = await _orderService.Create(newOrder);
            if (result == null)
                return BadRequest("Faild to create new order");
            return Ok(result);
        }

        [HttpGet("history/{id}")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "Customer,Seller")]
        public async Task<IActionResult> History(int id)
        {
            var result = await _orderService.History(id);
            if (result == null)
                return BadRequest("Wrong Id");
            return Ok(result);
        }

        [HttpGet("active/{id}")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "Customer,Seller")]
        public async Task<IActionResult> GetActiveOrders(int id)
        {
            var result = await _orderService.GetActiveOrders(id);
            if (result == null)
                return BadRequest("Wrong Id");
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "Administrator")]
        public async Task<IActionResult> AllOrder()
        {
            var result = await _orderService.AllOrders();
            return Ok(result);
        }

        //cancel Order
        [HttpPatch("cancel")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "Customer")]
        public async Task<IActionResult> CancelOrder(OrderCancelDto cancelOrder)
        {
            var result = await _orderService.CancelOrder(cancelOrder);
            if (!result)
                return BadRequest();
            return Ok();
        }
    }
}
