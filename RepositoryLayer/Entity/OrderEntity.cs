using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class Order
    {
        public int Order_Id { get; set; }
        public int Customer_Id { get; set; }
        public DateTime Order_Date { get; set; }
        public string Address { get; set; }
    }

    public class OrderItem
    {
        public int Order_Item_Id { get; set; }
        public int Order_Id { get; set; }
        public int Book_Id {  get; set; }
        public int Quantity { get; set; }
    }
}
