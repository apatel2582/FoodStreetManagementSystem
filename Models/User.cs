using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FoodStreetManagementSystem.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? UserType { get; set; }

        // Navigation property
        public List<Order>? Orders { get; set; }
    }
}
