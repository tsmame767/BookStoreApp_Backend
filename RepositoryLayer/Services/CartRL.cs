using Dapper;
using ModelLayer.DTO;
using RepositoryLayer.Database;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class CartRL:ICartRL
    {
        private readonly DBContext _dBContext;

        public CartRL(DBContext dbContext)
        {
            _dBContext = dbContext;
        }

        public async Task<List<ShoppingCartItem>> GetCartItems(int UserId)
        {
            var query = "select * from ShoppingCartItem where cart_Id=@cartId";
            var query2 = "select cart_id from ShoppingCart where User_Id = @UserId";
            //int CartId = 0;
            using (var connect = _dBContext.CreateConnection())
            {
                var CartId = await connect.QueryFirstOrDefaultAsync<int>(query2, new { UserId = UserId });
                var ListOfBooks = await connect.QueryAsync<ShoppingCartItem>(query, new { cartId = CartId });
                if (ListOfBooks == null)
                {
                    return null;
                }
                return ListOfBooks.ToList();
            }
        }
        public async Task<ShoppingCartItem> AddToCart(int UserId, ShoppingCartItemModel ItemModel)
        {
            var res = 0;
            var query = "insert into ShoppingCartItem(cart_id, book_id, quantity, Price) values(@cartId, @bookId, @quantity, @Price)";
            var query2 = "select cart_id from ShoppingCart where User_Id = @UserId"; //Query Getting cart id for specifc user
            using (var connect = _dBContext.CreateConnection())
            {
                var CartId = await connect.QueryFirstOrDefaultAsync<int>(query2, new {UserId = UserId}); //Cart Id of user who got an empty cart when user registered
                var Price = await connect.QueryFirstOrDefaultAsync<int>("select price from book where book_id=@bookId",new {bookId = ItemModel.Book_Id});

                res = await connect.ExecuteAsync(query, new { 
                                cartId = CartId,
                                bookId = ItemModel.Book_Id,
                                quantity = ItemModel.Quantity,
                                Price = Price
                });
                if(res == 0)
                {
                    throw new Exception("Couldn't Add to Cart!");
                }

                var CurrentCartData = await connect.QueryFirstOrDefaultAsync<ShoppingCartItem>("select Top 1 * from ShoppingCartItem order by cart_item_id desc");
                if(CurrentCartData == null)
                {
                    return null;
                }
                return CurrentCartData;

            }
        }

        public async Task<bool> RemoveFromCart(int CartItemId)
        {
            var res = 0;
            var query = "delete from ShoppingCartItem where Cart_Item_Id=@cartItemId";

            using(var connect  = _dBContext.CreateConnection())
            {
                res = await connect.ExecuteAsync(query, new { cartItemId = CartItemId });
                if (res == 0)
                {
                    throw new Exception("Couldn't Remove from Cart!");
                }
                return  true;
            }
        }

        public async Task<ShoppingCartItem> UpdateCart(int CartItemId, int quantity)
        {

            var res = 0;
            var query = "update ShoppingCartItem set quantity=@quantity where Cart_Item_Id=@cartItemId";

            using (var connect = _dBContext.CreateConnection())
            {
                res = await connect.ExecuteAsync(query, new { quantity = quantity, cartItemId = CartItemId });
                if (res == 0)
                {
                    throw new Exception("Couldn't Update Cart!");
                }
                var CurrentCartUpdateData = await connect.QueryFirstOrDefaultAsync<ShoppingCartItem>("select * from ShoppingCartItem where Cart_Item_Id = @cartItemId", new { cartItemId = CartItemId});
                if (CurrentCartUpdateData == null)
                {
                    return null;
                }
                return CurrentCartUpdateData;

            }
        }
    }
}
