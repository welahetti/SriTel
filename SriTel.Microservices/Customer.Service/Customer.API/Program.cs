using Customers.Infrastructure;
using Customers.Application;

var builder = WebApplication.CreateBuilder(args);

// Register CustomerDbContext using AddDbContext
builder.Services.AddScoped<CustomerDbContext>(provider =>
{
    var factory = new DesignTimeDbContextFactory(); // Create an instance of the factory
    return factory.CreateDbContext(args: null);     // Use the factory to create the DbContext
});


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register HttpClient
builder.Services.AddHttpClient("CustomerGateway", client =>
{
    client.BaseAddress = new Uri("https://localhost:7289"); // Replace with the actual URL of Mock.CustomerGateway
});

// Register dependencies
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

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
