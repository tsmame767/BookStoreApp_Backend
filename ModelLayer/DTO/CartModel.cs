using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTO
{
    public class CartModel
    {
        public int Customer_Id { get; set; }
    }

    public class ShoppingCartItemModel
    {
        public int Cart_Id { get; set; }
        public int Book_Id { get; set; }
        public int Quantity { get; set; }
    }
}
