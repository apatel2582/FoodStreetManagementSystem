﻿using System.Collections.Generic;
using System.Threading.Tasks;
using FoodStreetManagementSystem.Models;

namespace FoodStreetManagementSystem.DAL;

public interface IOrderRepository
{
    Task<List<Order>> GetOrdersAsync();

    Task<Order> GetOrderByIdAsync(int id);

    Task<bool> AddOrderAsync(Order order);

    Task<bool> UpdateOrderAsync(Order order);

    Task<bool> DeleteOrderAsync(int id);
}