using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using SriTel.ApiGateway;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Ocelot
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);

// Add controllers
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SriTel API Gateway",
        Version = "v1",
        Description = "API Gateway for SriTel Microservices using Ocelot."
    });
});

// Add HttpClient for DI
builder.Services.AddHttpClient();

var app = builder.Build();

// Custom Middleware
app.UseMiddleware<SwaggerAggregatorMiddleware>();

// Enable Swagger in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gateway with Downstream Services");
    });
}

// CORS
app.UseCors(policy =>
    policy.WithOrigins("http://localhost:8080")
          .AllowAnyHeader()
          .AllowAnyMethod());

// Routing
app.UseRouting();

// Authorization Middleware (if required)
app.UseAuthorization();

// Map Controllers
app.MapControllers();

// Ocelot Middleware
await app.UseOcelot();

app.Run();
