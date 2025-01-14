using Billing.Service.Billing.Infrastructure;
using SriTel.Billing.Application.Services;
using SriTel.Billing.Application.Services.Interfaces;
using SriTel.Billing.Repositories.Implementations;
using SriTel.Billing.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
