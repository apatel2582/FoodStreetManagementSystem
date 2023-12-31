﻿using System.Collections.Generic;
using System.Threading.Tasks;
using FoodStreetManagementSystem.Models;

namespace FoodStreetManagementSystem.DAL;

public interface IBillRepository
{
    Task<List<Bill>> GetBillsAsync();

    Task<Bill> GetBillByIdAsync(int id);

    Task<bool> AddBillAsync(Bill bill);

    Task<bool> UpdateBillAsync(Bill bill);

    Task<bool> DeleteBillAsync(int id);
}