using Billing.Domain;
using Billing.Service.Billing.Domain;
using Microsoft.EntityFrameworkCore;

namespace Billing.Service.Billing.Infrastructure
{
    public class BillingDbContext : DbContext
    {
        public BillingDbContext(DbContextOptions<BillingDbContext> options) : base(options) { }

        public DbSet<Bill> Bills { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Bill>()
                .HasMany(b => b.Payments)
                .WithOne()
                .HasForeignKey(p => p.BillID);

        }
    }
}