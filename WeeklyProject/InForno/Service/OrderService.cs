using InForno.Context;
using InForno.Dto;
using InForno.Interfaces;
using InForno.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace InForno.Service
{
    public class OrderService : IOrderService
    {
        private readonly InFornoDbContext _context;

        public OrderService(InFornoDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Product).ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Product).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<OrderDto> CreateOrderAsync(OrderDto orderDto)
        {
            var order = new Order
            {
                UserId = orderDto.UserId,
                OrderDate = orderDto.OrderDate,
                ShippingAddress = orderDto.ShippingAddress,
                Notes = orderDto.Notes,
                IsProcessed = orderDto.IsProcessed,
                OrderDetails = orderDto.OrderDetails.Select(od => new OrderDetail
                {
                    ProductId = od.ProductId,
                    Quantity = od.Quantity
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            orderDto.Id = order.Id;
            return orderDto;
        }

        public async Task<Order> UpdateOrderAsync(int id, OrderDto orderDto)
        {
            var order = await _context.Orders.Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return null;
            }

            order.UserId = orderDto.UserId;
            order.OrderDate = orderDto.OrderDate;
            order.ShippingAddress = orderDto.ShippingAddress;
            order.Notes = orderDto.Notes;
            order.IsProcessed = orderDto.IsProcessed;

            // Update order details
            order.OrderDetails.Clear();
            order.OrderDetails = orderDto.OrderDetails.Select(od => new OrderDetail
            {
                ProductId = od.ProductId,
                Quantity = od.Quantity
            }).ToList();

            await _context.SaveChangesAsync();
            return order;
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateOrderStatusAsync(int id, bool isProcessed)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.IsProcessed = isProcessed;
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetTotalOrdersProcessedAsync(DateTime date)
        {
            return await _context.Orders.CountAsync(o => o.IsProcessed && o.OrderDate.Date == date.Date);
        }

        public async Task<decimal> GetTotalRevenueAsync(DateTime date)
        {
            return await _context.Orders
                .Where(o => o.IsProcessed && o.OrderDate.Date == date.Date)
                .SelectMany(o => o.OrderDetails)
                .SumAsync(od => od.Product.Price * od.Quantity);
        }
    }
}
