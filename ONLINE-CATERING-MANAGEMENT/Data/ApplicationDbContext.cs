using Microsoft.EntityFrameworkCore;
using ONLINE_CATERING_MANAGEMENT.Models;

namespace ONLINE_CATERING_MANAGEMENT.Data
{
    public class ApplicationDbContext : DbContext
    {
        // This constructor receives connection settings from Program.cs
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // This represents the Customers table in PostgreSQL
        public DbSet<Customer> Customers { get; set; }
    }
}