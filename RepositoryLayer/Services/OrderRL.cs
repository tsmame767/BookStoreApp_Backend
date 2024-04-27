using Dapper;
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
    public class OrderRL:IOrderRL
    {
        private readonly DBContext _dBContext;

        public OrderRL(DBContext dbContext)
        {
            _dBContext = dbContext;
        }

        public async Task<bool> PlaceOrder(int userId, string address)
        {
            var connection = _dBContext.CreateConnection();
            try
            {
                connection.Open(); // Open the connection before using it
                using (var transaction = connection.BeginTransaction())
                {
                    // Step 1: Insert new Order record
                    var orderInsertQuery = "INSERT INTO [Order] (User_Id, Order_Date, Address) VALUES (@UserId, @OrderDate, @Address); SELECT SCOPE_IDENTITY();";


                    //have to write conditional code if user exist already then have to just update the row and if not then insert new row
                    var orderId = await connection.ExecuteScalarAsync<int>(
                        orderInsertQuery,
                        new { UserId = userId, OrderDate = DateTime.UtcNow, Address = address },
                        transaction
                    );



                    // Step 2: Retrieve all items from the user's cart
                    var cartItemsQuery = @"
         SELECT * FROM ShoppingCartItem 
         WHERE Cart_Id = (SELECT Cart_Id FROM ShoppingCart WHERE user_Id = @UserId)";
                    var cartItems = (await connection.QueryAsync<ShoppingCartItem>(cartItemsQuery, new { UserId = userId }, transaction)).ToList();
                    // Check if the cart is empty
                    if (!cartItems.Any()) //!false
                    {
                        throw new InvalidOperationException("The cart is empty.");
                    }

                    // Convert cart items to order items
                    //var orderItems = cartItems.Select(cartItem => new
                    //{
                    //    Order_Id = orderId,
                    //    Book_Id = cartItem.Book_Id,
                    //    Quantity = cartItem.Quantity
                    //}).ToList();

                    // Step 3: Bulk insert cart items into OrderItem table
                    var orderItemsInsertQuery = $@"
     INSERT INTO OrderItem (Order_Id, Book_Id, Quantity, Price)
     VALUES ({orderId}, @Book_Id, @Quantity, @Price)";
                    if (cartItems.Any()) //true
                    {
                        await connection.ExecuteAsync(orderItemsInsertQuery, cartItems, transaction);
                    }

                    // Step 4: Calculate total amount
                    int totalAmount = cartItems.Sum(item => item.Price * item.Quantity);

                    // Step 5: Update Order with total amount
                    var updateOrderAmountQuery = "UPDATE [Order] SET Amount = @Amount WHERE Order_Id = @OrderId";
                    await connection.ExecuteAsync(updateOrderAmountQuery, new { Amount = totalAmount, OrderId = orderId }, transaction);

                    // Step 6: Clear the cart
                    var clearCartQuery = "DELETE FROM ShoppingCartItem WHERE Cart_Id = (SELECT Cart_Id FROM ShoppingCart WHERE User_Id = @UserId)";
                    await connection.ExecuteAsync(clearCartQuery, new { UserId = userId }, transaction);

                    // Commit transaction if all steps are successful
                    transaction.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Log the exception details here or handle them as needed
                throw new Exception("An error occurred while placing the order: " + ex.Message, ex);
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close(); // Ensure the connection is closed after operation
            }
        }
    }
}
