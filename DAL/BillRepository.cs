using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodStreetManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodStreetManagementSystem.DAL;

public class BillRepository : IBillRepository
{
    private readonly AppDbContext _context;

    public BillRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Bill>> GetBillsAsync()
    {
        return await _context.Bills.ToListAsync();
    }

    public async Task<Bill> GetBillByIdAsync(int id)
    {

        var bill = await _context.Bills.FindAsync(id);
        return bill ?? throw new Exception($"No bill found with ID {id}");
    }

    //public async Task<bool> AddBillAsync(Bill bill)
    //{
    //    _context.Bills.Add(bill);
    //    var saveResult = await _context.SaveChangesAsync();
    //    return saveResult > 0;
    //}
    public async Task<bool> AddBillAsync(Bill bill)
    {
        var order = await _context.Orders.FindAsync(bill.OrderID);
        if (order == null || !order.IsFulfilled)
        {
            return false;
        }
        // Generate a unique BillNumber
        bill.BillNumber = "BIL" + (await _context.Bills.CountAsync() + 1).ToString("D3");  // "D3" formats the number as a 3-digit string, padding with zeros if necessary
        _context.Bills.Add(bill);
        var saveResult = await _context.SaveChangesAsync();
        return saveResult > 0;
    }


    //public async Task<bool> UpdateBillAsync(Bill bill)
    //{
    //    _context.Bills.Attach(bill);
    //    _context.Entry(bill).State = EntityState.Modified;
    //    var saveResult = await _context.SaveChangesAsync();
    //    return saveResult > 0;
    //}
    public async Task<bool> UpdateBillAsync(Bill bill)
    {
        var billToUpdate = await _context.Bills.FindAsync(bill.BillID);
        if (billToUpdate == null)
        {
            return false;
        }
        billToUpdate.IsPaid = bill.IsPaid;
        var saveResult = await _context.SaveChangesAsync();
        return saveResult > 0;
    }


    //public async Task<bool> DeleteBillAsync(int id)
    //{
    //    var bill = await _context.Bills.FindAsync(id);
    //    if (bill == null)
    //    {
    //        return false;
    //    }

    //    _context.Bills.Remove(bill);
    //    var saveResult = await _context.SaveChangesAsync();
    //    return saveResult > 0;
    //}
    public async Task<bool> DeleteBillAsync(int id)
    {
        var bill = await _context.Bills.FindAsync(id);
        if (bill == null || bill.IsPaid)
        {
            return false;
        }
        _context.Bills.Remove(bill);
        var saveResult = await _context.SaveChangesAsync();
        return saveResult > 0;
    }

}