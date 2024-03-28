using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAssignment3.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        // Navigation property for the user associated with this cart
        public User User { get; set; }

        // Collection navigation property for the products in the cart
        public ICollection<CartItem> CartItems { get; set; }
    }

    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CartId { get; set; }

        // Navigation property for the cart containing this item
        public Cart Cart { get; set; }

        [Required]
        public int ProductId { get; set; }

        // Navigation property for the product in this item
        public Product Product { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
