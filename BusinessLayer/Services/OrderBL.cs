using BusinessLayer.Interfaces;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class OrderBL:IOrderBL
    {
        private readonly IOrderRL _orderService;

        public OrderBL(IOrderRL orderService)
        {
            _orderService = orderService;
        }

        public Task<bool> PlaceOrder(int userId, string address)
        {
            return this._orderService.PlaceOrder(userId, address);

        }
    }
}
