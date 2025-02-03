using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Customers.Infrastructure
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CustomerDbContext>
    {
        public CustomerDbContext CreateDbContext(string[] args)
        {
            // Get the directory of the Web API project
            var webApiProjectDir = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Customer.Service", "Customer.API");

            // Build the configuration using the correct path to appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(webApiProjectDir)  // Set base path to the Web API project directory
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)  // Use appsettings.json from Web API
                .Build();

            var connectionString = configuration.GetConnectionString("CustomerDbConnection");

            var optionsBuilder = new DbContextOptionsBuilder<CustomerDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new CustomerDbContext(optionsBuilder.Options);
        }
    }
}
