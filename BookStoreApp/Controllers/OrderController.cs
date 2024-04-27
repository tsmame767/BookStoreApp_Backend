using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pipelines.Sockets.Unofficial.Buffers;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookStoreApp.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderBL _orderService;

        public OrderController(IOrderBL bookService)
        {
            _orderService = bookService;
        }

        [HttpPost("Place-Order")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> PlaceOrder(string Address)
        {
            try
            {
                var claim = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var res = await _orderService.PlaceOrder(claim, Address);
                if (res)
                {
                    return Ok(new
                    {
                        Success = true,
                        Message = "Order Placed!\n Below are the Order Details",
                        Data = res

                    });
                }
                return Ok(new
                {
                    Success = false,
                    Message = "Order Not Placed!",
                    

                });
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
