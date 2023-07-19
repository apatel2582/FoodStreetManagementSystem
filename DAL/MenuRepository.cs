using FoodStreetManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodStreetManagementSystem.DAL
{
    public class MenuRepository : IMenuRepository
    {
        private readonly AppDbContext _context;

        public MenuRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<MenuItem>> GetMenuItemsAsync()
        {
            return await _context.MenuItems.ToListAsync();
        }

        public async Task<MenuItem> GetMenuItemByIdAsync(int id)
        {
            var item = await _context.MenuItems.FindAsync(id);
            return item ?? throw new Exception($"No menu item found with ID {id}");
        }
        public async Task<bool> AddMenuItemAsync(MenuItem item)
        {
            _context.MenuItems.Add(item);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult > 0;
        }

        public async Task<bool> UpdateMenuItemAsync(MenuItem item)
        {
            _context.MenuItems.Attach(item);
            _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            var saveResult = await _context.SaveChangesAsync();
            return saveResult > 0;
        }

        public async Task<bool> DeleteMenuItemAsync(int id)
        {
            var item = await _context.MenuItems.FindAsync(id);
            if (item == null)
            {
                return false;
            }

            _context.MenuItems.Remove(item);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult > 0;
        }
    }
}