using ModelLayer.DTO;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface ICartRL
    {
        Task<List<ShoppingCartItem>> GetCartItems(int UserId);
        Task<ShoppingCartItem> AddToCart(int UserId, ShoppingCartItemModel ItemModel);
        Task<bool> RemoveFromCart(int CartItemId);
        Task<ShoppingCartItem> UpdateCart(int CartItemId, int quantity);
    }
}
