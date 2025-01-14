using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Billing.Service.Billing.Infrastructure
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BillingDbContext>
    {
        public BillingDbContext CreateDbContext(string[] args)
        {
            // Get the directory of the Web API project (make sure this matches your directory structure)
            var webApiProjectDir = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName + "\\");

            // Build the configuration using the correct path to appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(webApiProjectDir)  // Set base path to the Web API project directory
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)  // Use appsettings.json from Web API
                .Build();

            var connectionString = configuration.GetConnectionString("BillingDbConnection");

            var optionsBuilder = new DbContextOptionsBuilder<BillingDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new BillingDbContext(optionsBuilder.Options);
        }
    }
}
