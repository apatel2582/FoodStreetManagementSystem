using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FoodStreetManagementSystem.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public DateTime OrderTime { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsFulfilled { get; set; }
        public int? BillID { get; set; }

        // Navigation properties
        public User? User { get; set; }
        public Bill? Bill { get; set; }
        public List<OrderItem>? OrderItems { get; set; }
    }
}
