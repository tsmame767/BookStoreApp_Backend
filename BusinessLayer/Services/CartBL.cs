using BusinessLayer.Interfaces;
using ModelLayer.DTO;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class CartBL:ICartBL
    {
        private readonly ICartRL _cartService;

        public CartBL(ICartRL cartService)
        {
            _cartService = cartService;
        }

        public Task<List<ShoppingCartItem>> GetCartItems(int UserId)
        {
            return this._cartService.GetCartItems(UserId);
        }

        public Task<ShoppingCartItem> AddToCart(int UserId, ShoppingCartItemModel ItemModel)
        {
            return this._cartService.AddToCart(UserId, ItemModel);
        }

        public Task<bool> RemoveFromCart(int CartItemId)
        {
            return this._cartService.RemoveFromCart(CartItemId);
        }

        public Task<ShoppingCartItem> UpdateCart(int CartItemId, int quantity)
        {
            return this._cartService.UpdateCart(CartItemId, quantity);
        }
    }
}
