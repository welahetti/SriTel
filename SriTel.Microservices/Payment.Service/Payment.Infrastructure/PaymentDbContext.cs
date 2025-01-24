using Payment.Domain;
using Microsoft.EntityFrameworkCore;

namespace Payment.Infrastructure
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options) { }
       
        public DbSet<Bill> Bills { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Pay> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pay>().HasKey(p => p.PaymentID);
        }
    }

}
