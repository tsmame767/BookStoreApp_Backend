using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class BookEntity
    {
        public int Book_Id { get; set; }    //Primary Key
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public int quantity { get; set; }
        public string Image_Url { get; set; }

    }
}
