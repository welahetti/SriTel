using Customers.Domain;
using Microsoft.EntityFrameworkCore;

namespace Customers.Infrastructure
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options) { }
        public DbSet<Domain.Customer> Customers { get; set; }
        public DbSet<Bill> Bills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Example: Configure the primary key (this is usually not necessary if it's just the default convention)
            modelBuilder.Entity<Domain.Customer>()
                .HasKey(c => c.CustomerId);

            // Example: Configure column name for FullName to be non-nullable
            modelBuilder.Entity<Domain.Customer>()
                .Property(c => c.FullName)
                .IsRequired()
                .HasMaxLength(100);  // You can set a max length if needed

            // Example: Configure Email to be unique
            modelBuilder.Entity<Domain.Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();
        }
    }
}
