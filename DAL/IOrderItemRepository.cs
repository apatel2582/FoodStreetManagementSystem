using System.Collections.Generic;
using System.Threading.Tasks;
using FoodStreetManagementSystem.Models;

namespace FoodStreetManagementSystem.DAL;

public interface IOrderItemRepository
{
    Task<List<OrderItem>> GetOrderItemsAsync();

    Task<OrderItem> GetOrderItemByIdAsync(int id);

    Task<bool> AddOrderItemAsync(OrderItem orderItem);

    Task<bool> UpdateOrderItemAsync(OrderItem orderItem);

    Task<bool> DeleteOrderItemAsync(int id);
}