using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Payments.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Payments.Service.Payment.Infrastructure
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PaymentDbContext>
    {
        public PaymentDbContext CreateDbContext(string[] args)
        {
            // Get the directory of the Web API project (make sure this matches your directory structure)
            var webApiProjectDir = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName + "\\Payment.API\\");

            // Build the configuration using the correct path to appsettings.json
            var configuration = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath(webApiProjectDir)  // Set base path to the Web API project directory
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)  // Use appsettings.json from Web API
                .Build();

            var connectionString = configuration.GetConnectionString("PaymentDbConnection");

            var optionsBuilder = new DbContextOptionsBuilder<PaymentDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new PaymentDbContext(optionsBuilder.Options);
        }
    }
}
