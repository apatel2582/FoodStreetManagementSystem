using FoodStreetManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStreetManagementSystem.DAL
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user ?? throw new Exception($"No user found with ID {id}");
        }

        public async Task<bool> AddUserAsync(User user)
        {
            _context.Users.Add(user);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult > 0;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _context.Users.Attach(user);
            _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            var saveResult = await _context.SaveChangesAsync();
            return saveResult > 0;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult > 0;
        }

        public async Task<User?> ValidateCredentials(string username, string password)
        {
            var user = await _context.Users
                .Where(u => u.UserName == username && u.Password == password)
                .FirstOrDefaultAsync();
            return user;
        }

    }
}
