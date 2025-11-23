using MiniYemekSepeti.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace MiniYemekSepeti.Services
{
    // This class is used for interacting with the database, inheriting from IdentityDbContext to include authentication-related tables.
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // Constructor to pass the options to the base class (IdentityDbContext) for database configuration
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        // Define DbSets for each entity to be mapped to a database table
        public DbSet<Food> Food { get; set; }           // Table for food items
        public DbSet<Order> Orders { get; set; }        // Table for orders
        public DbSet<OrderLog> OrderLogs { get; set; }  // Table for order logs
        public DbSet<Notification> Notifications { get; set; } // Table for notifications

        // Define DbSet for OrderDetailsViewModel, typically used for custom views or non-entity data
        public DbSet<OrderDetailsViewModel> OrderDetailsViews { get; set; }

        // Configuring the model further
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure OrderDetailsView to represent a view in the database (not an actual table)
            modelBuilder.Entity<OrderDetailsViewModel>(entity =>
            {
                entity.HasNoKey();  // No primary key for the view
                entity.ToView("OrderDetailsView");  // Specify the SQL view to be used
            });

            // Call base method to apply other configurations from IdentityDbContext
            base.OnModelCreating(modelBuilder);
        }
    }
}
