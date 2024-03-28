using System.ComponentModel.DataAnnotations;

namespace WebAssignment3.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Username { get; set; }
        public required string PurchaseHistory { get; set; }
        public required string ShippingAddress { get; set; }


        // Inside the User class
        public ICollection<Comment> Comments { get; set; }

    }
}
