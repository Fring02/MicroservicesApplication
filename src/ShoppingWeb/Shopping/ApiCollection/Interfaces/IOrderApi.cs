using Shopping.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.ApiCollection.Interfaces
{
    public interface IOrderApi
    {
        Task<IEnumerable<Order>> GetOrdersByUsername(string username);
        Task<bool> DeleteOrderById(int id);
        Task<Order> GetOrderById(int id);
        Task Checkout(Order order);
    }
}
