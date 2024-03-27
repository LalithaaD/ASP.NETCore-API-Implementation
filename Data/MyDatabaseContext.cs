using Microsoft.EntityFrameworkCore;
using WebAssignment3.Models;

namespace WebAssignment3.Data
{
    public class MyDatabaseContext : DbContext
    {
        public MyDatabaseContext(DbContextOptions<MyDatabaseContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } 

    }
}
