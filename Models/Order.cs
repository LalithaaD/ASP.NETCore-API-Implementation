using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAssignment3.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        // Navigation property for the user who made the order
        public User User { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        // Navigation property for the order items associated with this order
        public ICollection<OrderItem> OrderItems { get; set; }
    }

    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        // Navigation property for the order to which this item belongs
        public Order Order { get; set; }

        [Required]
        public int ProductId { get; set; }

        // Navigation property for the product in this order item
        public Product Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
