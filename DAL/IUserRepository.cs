using FoodStreetManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodStreetManagementSystem.DAL
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync();

        Task<User> GetUserByIdAsync(int id);

        Task<bool> AddUserAsync(User user);

        Task<bool> UpdateUserAsync(User user);

        Task<bool> DeleteUserAsync(int id);

        Task<User?> ValidateCredentials(string username, string password);
    }
}