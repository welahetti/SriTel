using Microsoft.EntityFrameworkCore;

namespace Customers.Infrastructure
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options) { }

        //public DbSet<Bill> Bills { get; set; }
        //public DbSet<Payment> Payments { get; set; }
        //public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //// Configure relationships and set delete behavior to prevent multiple cascade paths
            //modelBuilder.Entity<Bill>()
            //    .HasMany(b => b.Payments)
            //    .WithOne(p => p.Bill)  // Explicitly define the relationship on the Payment side
            //    .HasForeignKey(p => p.BillID)
            //    .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete from Bill to Payment

            //modelBuilder.Entity<Payment>()
            //    .HasOne(p => p.Bill)
            //    .WithMany(b => b.Payments)
            //    .HasForeignKey(p => p.BillID)
            //    .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete from Payment to Bill
        }
    }
}
