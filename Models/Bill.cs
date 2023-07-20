using System;
using System.ComponentModel.DataAnnotations;

namespace FoodStreetManagementSystem.Models;

public class Bill
{
    [Key]
    public int BillID { get; set; }
    public int OrderID { get; set; }
    public DateTime BillTime { get; set; }
    public decimal AmountDue { get; set; }

    // Navigation property
    public Order? Order { get; set; }
}