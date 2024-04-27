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

       
    }
}
