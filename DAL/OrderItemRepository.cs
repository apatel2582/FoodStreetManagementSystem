using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodStreetManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodStreetManagementSystem.DAL;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly AppDbContext _context;

    public OrderItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrderItem>> GetOrderItemsAsync()
    {
        return await _context.OrderItems
        .Include(oi => oi.Order)
        .Include(oi => oi.MenuItem)
        .ToListAsync();
    }

    public async Task<OrderItem> GetOrderItemByIdAsync(int id)
    {
        var orderItem = await _context.OrderItems.FindAsync(id);
        return orderItem ?? throw new Exception($"No order item found with ID {id}");
    }
    public async Task<bool> AddOrderItemAsync(OrderItem orderItem)
    {
        _context.OrderItems.Add(orderItem);
        var saveResult = await _context.SaveChangesAsync();
        return saveResult > 0;
    }
    public async Task<bool> UpdateOrderItemAsync(OrderItem orderItem)
    {
        _context.OrderItems.Attach(orderItem);
        _context.Entry(orderItem).State = EntityState.Modified;
        var saveResult = await _context.SaveChangesAsync();
        return saveResult > 0;
    }
    public async Task<bool> DeleteOrderItemAsync(int id)
    {
        var orderItem = await _context.OrderItems.FindAsync(id);
        if (orderItem == null)
        {
            return false;
        }

        _context.OrderItems.Remove(orderItem);
        var saveResult = await _context.SaveChangesAsync();
        return saveResult > 0;
    }
}