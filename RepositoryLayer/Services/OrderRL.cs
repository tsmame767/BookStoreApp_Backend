using RepositoryLayer.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class OrderRL
    {
        private readonly DBContext _dBContext;

        public OrderRL(DBContext dbContext)
        {
            _dBContext = dbContext;
        }

        public async Task<bool> PlaceOrder()
        {
            return false;
        }
    }
}
