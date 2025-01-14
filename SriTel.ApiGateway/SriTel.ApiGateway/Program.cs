using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using SriTel.ApiGateway;

var builder = WebApplication.CreateBuilder(args);

// Add logging services
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// **Add Ocelot services**: Ocelot is used for API Gateway routing
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);

// **Add services to the container**: Controllers are required for API routes
builder.Services.AddControllers();

// **Add Swagger services**: This will generate the Swagger UI and OpenAPI documentation
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

var app = builder.Build();

// **Middleware**: Custom middleware for Swagger aggregation if needed
app.UseMiddleware<SwaggerAggregatorMiddleware>();

// **Configure the HTTP request pipeline**

if (app.Environment.IsDevelopment())
{
    // Enable Swagger and Swagger UI in development mode
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gateway with Downstream Services");
    });
}
app.UseCors(builder =>
    builder.WithOrigins("http://localhost:8080") // Frontend URL
           .AllowAnyHeader()
           .AllowAnyMethod());
// **Add Routing**: Enable routing to map requests to controllers
app.UseRouting();

// **Authorization Middleware**: Optional, depending on your authorization setup
app.UseAuthorization();

// **Map Controllers**: Map the API controllers defined in your project
app.MapControllers();

// **Configure Ocelot middleware**: Ocelot will handle API Gateway routing
app.UseOcelot().Wait();

app.Run();
