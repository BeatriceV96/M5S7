using InForno.Dto;
using InForno.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InForno.Interfaces
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task<OrderDto> CreateOrderAsync(OrderDto orderDto);
        Task<Order> UpdateOrderAsync(int id, OrderDto orderDto);
        Task DeleteOrderAsync(int id);
    }
}
