using FoodStreetManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodStreetManagementSystem.DAL
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            return order ?? throw new Exception($"No order found with ID {id}");
        }

        public async Task<bool> AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult > 0;
        }

        public async Task<bool> UpdateOrderAsync(Order order)
        {
            _context.Orders.Attach(order);
            _context.Entry(order).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            var saveResult = await _context.SaveChangesAsync();
            return saveResult > 0;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return false;
            }

            _context.Orders.Remove(order);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult > 0;
        }
    }
}
