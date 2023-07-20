using System.ComponentModel.DataAnnotations;

namespace FoodStreetManagementSystem.Models;

public class MenuItem
{
    [Key]
    public int MenuItemID { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public string? ImageURL { get; set; }
    public bool IsActive { get; set; }
}