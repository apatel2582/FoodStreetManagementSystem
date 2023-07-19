using FoodStreetManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodStreetManagementSystem.DAL
{
    public interface IMenuRepository
    {
        Task<List<MenuItem>> GetMenuItemsAsync();
        Task<MenuItem> GetMenuItemByIdAsync(int id);
        Task<bool> AddMenuItemAsync(MenuItem item);
        Task<bool> UpdateMenuItemAsync(MenuItem item);
        Task<bool> DeleteMenuItemAsync(int id);
    }
}
