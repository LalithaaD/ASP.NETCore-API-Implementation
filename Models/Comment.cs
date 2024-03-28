using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using WebAssignment3.Data;

namespace WebAssignment3.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        // Navigation property for the product associated with this comment
        public Product Product { get; set; }

        [Required]
        public int UserId { get; set; }

        // Navigation property for the user who made this comment
        public User User { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        public string Image { get; set; }

        [MaxLength(1000)]
        public string Text { get; set; }
    }

}