﻿using System.ComponentModel.DataAnnotations;

namespace FoodStreetManagementSystem.Models;

public class OrderItem
{
    [Key]
    public int OrderItemID { get; set; }
    public int OrderID { get; set; }
    public int MenuItemID { get; set; }
    public int Quantity { get; set; }
    public decimal Subtotal { get; set; }  // Add this line

    // Navigation properties
    public Order? Order { get; set; }
    public MenuItem? MenuItem { get; set; }
}