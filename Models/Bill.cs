using System;
using System.ComponentModel.DataAnnotations;

namespace FoodStreetManagementSystem.Models;

public class Bill
{
    [Key]
    public int BillID { get; set; }
    public int OrderID { get; set; }
    public DateTime BillTime { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsPaid { get; set; }
    public string BillNumber { get; set; }  // Added this line
    // Navigation property
    //public Order? Order { get; set; }
    public virtual Order Order { get; set; }
}