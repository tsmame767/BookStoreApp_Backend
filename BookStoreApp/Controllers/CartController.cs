using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO;
using RepositoryLayer.Entity;
using System.Security.Claims;

namespace BookStoreApp.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartBL _cartService;

        public CartController(ICartBL cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("GetAllItems")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> GetAllCartItems()
        {
            var claim = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                var result = await _cartService.GetCartItems(claim);
                if (result == null)
                {
                    return Ok(new
                    {
                        Success = false,
                        Message = "Item Not in Cart Available!",
                        Data = result
                    });
                }
                return Ok(new
                {
                    Success = true,
                    Message = "Item in Cart!",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("Add-Item-cart")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> AddItemToCart(ShoppingCartItemModel itemModel)
        {
            try
            {
                var claim = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var result = await _cartService.AddToCart(claim, itemModel);
                if (result == null)
                {
                    return Ok(new
                    {
                        Success = false,
                        Message = "Item Not Added to Cart!",
                        Data = result
                    });
                }
                return Ok(new
                {
                    Success = true,
                    Message = "Item Added to Cart!",
                    Data = result
                });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete-From-Cart")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> RemoveItemFromCart(int CartItemId)
        {
            try
            {
                var result = await _cartService.RemoveFromCart(CartItemId);
                if (result == false)
                {
                    return Ok(new
                    {
                        Success = result,
                        Message = "Item Not Deleted from Cart!"

                    });
                }
                return Ok(new
                {
                    Success = result,
                    Message = "Item Deleted from Cart!"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("Update-Cart")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> UpdateToCart(int CartItemId, int quantity)
        {
            if (quantity <= 0)
            {
                return BadRequest("Quantity must be greater than zero.");
            }
            try
            {
                var result = await _cartService.UpdateCart(CartItemId, quantity);
                if (result == null)
                {
                    return Ok(new
                    {
                        Success = result,
                        Message = "Item Not Updated from Cart!"

                    });
                }
                return Ok(new
                {
                    Success = result,
                    Message = "Item Updated from Cart!"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
