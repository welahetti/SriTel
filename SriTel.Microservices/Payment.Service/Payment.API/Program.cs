using Payments.Application;
using Payments.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Payment.API.MessageBroker;
using Payment.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add RabbitMQ consumer as a background service
builder.Services.AddHostedService<RabbitMQConsumer>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register PaymentDbContext using AddDbContext
builder.Services.AddDbContext<PaymentDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("PaymentDbConnection"));
});

// Register dependencies
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

// Add CORS policy for Vue.js frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp", policy =>
    {
        policy.WithOrigins("http://localhost:8080") // Replace with your Vue.js URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowVueApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
