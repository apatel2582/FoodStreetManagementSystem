using System.Collections.Generic;
using System.Threading.Tasks;
using FoodStreetManagementSystem.Models;

namespace FoodStreetManagementSystem.DAL;

public interface IUserRepository
{
    Task<List<User>> GetUsersAsync();

    Task<User> GetUserByIdAsync(int id);

    Task<bool> AddUserAsync(User user);

    Task<bool> UpdateUserAsync(User user);

    Task<bool> DeleteUserAsync(int id);

    Task<User?> ValidateCredentials(string username, string password);
}