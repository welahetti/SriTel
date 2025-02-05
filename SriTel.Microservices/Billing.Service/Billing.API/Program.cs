using Billing.API.MessageBroker;
using Billing.Repositories.Implementations;
using Billing.Repositories.Interfaces;
using Billing.Service.Billing.Infrastructure;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SriTel.Billing.Application.Services.Interfaces;
using SriTel.Billing.Application.Services;
using Billing.API.MessageBroker.Configuration;
using Billing.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
// Ensure appsettings.json is explicitly added to the configuration
//builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Use the DesignTimeDbContextFactory to create BillingDbContext
builder.Services.AddScoped<BillingDbContext>(provider =>
{
    var factory = new DesignTimeDbContextFactory(); // Create an instance of the factory
    return factory.CreateDbContext(args: null);     // Use the factory to create the DbContext
});

// Register dependencies
builder.Services.AddScoped<IBillingRepository, BillingRepository>();
builder.Services.AddScoped<IBillingService, BillingService>();

// Configure RabbitMQ settings
builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));

// Register RabbitMQPublisher with dependencies
builder.Services.AddSingleton<RabbitMQPublisher>(provider =>
{
    var options = provider.GetRequiredService<IOptions<RabbitMQSettings>>(); // Get RabbitMQ settings
    var factory = new ConnectionFactory()
    {
        HostName = "localhost",
        UserName = "guest",
        Password = "guest"
    };

    return new RabbitMQPublisher(options, factory); // Create and return RabbitMQPublisher instance
});

// Configure the HTTP request pipeline.
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
