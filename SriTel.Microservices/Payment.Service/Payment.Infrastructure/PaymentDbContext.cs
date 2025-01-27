using Payments.Domain;
using Microsoft.EntityFrameworkCore;

namespace Payments.Infrastructure
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options) { }
       
        public DbSet<Bill> Bills { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Domain.Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Domain.Payment>(entity =>
            {
                entity.HasKey(p => p.PaymentID);
                entity.Property(p => p.AmountPaid).IsRequired();
                entity.Property(p => p.PaymentDate).IsRequired();
            });
        }
    }

}
