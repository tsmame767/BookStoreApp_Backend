using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTO
{
    public class OrderModel
    {
        public int Customer_Id { get; set; }
        public DateTime Order_Date { get; set; }
        public string Address { get; set; }
    }

    public class OrderItemModel
    {
        public int Order_Id { get; set; }
        public int Book_Id { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderSummary
    {
        public int Order_Id { get; set; } //OrderItemModel
        public int Book_Id { get; set; } //OrderItemModel
        public int Quantity { get; set; } //OrderItemModel

        public string Title {  get; set; } //Book
        public string Author { get; set; } //Book
        public string Image_url {  get; set; } //Book
        public string Amount { get; set; } //Book | //CartItem
    }
}
