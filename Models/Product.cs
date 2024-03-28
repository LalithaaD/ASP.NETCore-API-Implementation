using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAssignment3.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        public string Image { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Pricing { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ShippingCost { get; set; }

        // Inside the Product class
        public ICollection<Comment> Comments { get; set; }
    }
}
