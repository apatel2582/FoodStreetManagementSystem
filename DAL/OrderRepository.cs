using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStreetManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodStreetManagementSystem.DAL;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.Bill)
            .Include(o => o.OrderItems)  // Add this line
            .ToListAsync();
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
        _context.Entry(order).State = EntityState.Modified;
        var saveResult = await _context.SaveChangesAsync();
        return saveResult > 0;
    }

    public async Task<bool> DeleteOrderAsync(int id)
    {
        //var order = await _context.Orders.FindAsync(id);
        var order = await _context.Orders.Include(o => o.OrderItems).Include(o => o.Bill).FirstOrDefaultAsync(o => o.OrderID == id);
        if (order == null)
        {
            return false;
        }

        // Check if the order is fulfilled, has order items, or has a bill
        if (order.IsFulfilled || order.OrderItems.Any() || order.Bill != null)
        {
            // If any of these conditions are true, do not delete the order
            return false;
        }

        _context.Orders.Remove(order);
        var saveResult = await _context.SaveChangesAsync();
        return saveResult > 0;
    }
}