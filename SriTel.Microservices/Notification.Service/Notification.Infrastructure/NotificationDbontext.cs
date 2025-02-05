using Microsoft.EntityFrameworkCore;
using Notification.Domain;

namespace Notification.Infrastructure
{
    public class NotificationDbContext : DbContext
        {
            public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options) { }
            public DbSet<FcmToken> FcmTokens { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

               
            }
        }

    }

