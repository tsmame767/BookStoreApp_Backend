using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class Cart
    {
        public int Cart_Id { get; set; }
        public int Customer_Id { get; set; }
    }

    public class ShoppingCartItem
    {
        public int Cart_Item_Id { get; set; }
        public int Cart_Id { get; set; }
        public int Book_Id { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}
