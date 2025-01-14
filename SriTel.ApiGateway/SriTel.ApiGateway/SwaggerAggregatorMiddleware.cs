namespace SriTel.ApiGateway
{
    public class SwaggerAggregatorMiddleware
    {
        private readonly RequestDelegate _next;

        public SwaggerAggregatorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine($"Request Path: {context.Request.Path}");
            if (context.Request.Path.StartsWithSegments("/swagger/v1/swagger.json"))
            {
                Console.WriteLine("Matched /swagger/v1/swagger.json");
                var billingSwagger = await new HttpClient().GetStringAsync("http://localhost:5205/swagger/v1/swagger.json");

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(billingSwagger);
                return;
            }

            await _next(context);
        }
    }
}