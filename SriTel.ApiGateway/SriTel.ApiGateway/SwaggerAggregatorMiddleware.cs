using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SriTel.ApiGateway
{
    public class SwaggerAggregatorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public SwaggerAggregatorMiddleware(RequestDelegate next, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _next = next;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Equals("/swagger/v1/swagger.json", StringComparison.OrdinalIgnoreCase))
            {
                var billingServiceUrl = _configuration["Services:Billing"];
                var paymentServiceUrl = _configuration["Services:Payment"];
                var client = _httpClientFactory.CreateClient();

                try
                {
                    var billingSwagger = await client.GetStringAsync($"{billingServiceUrl}/swagger/v1/swagger.json");
                    var paymentSwagger = await client.GetStringAsync($"{paymentServiceUrl}/swagger/v1/swagger.json");

                    var mergedSwagger = MergeSwaggerDocuments(billingSwagger, paymentSwagger);

                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(mergedSwagger);
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync($"Error: {ex.Message}");
                }

                return;
            }

            await _next(context);
        }
        private string MergeSwaggerDocuments(string billingSwagger, string paymentSwagger)
        {
            dynamic billingDoc = Newtonsoft.Json.JsonConvert.DeserializeObject(billingSwagger);
            dynamic paymentDoc = Newtonsoft.Json.JsonConvert.DeserializeObject(paymentSwagger);

            // Merge paths
            foreach (var path in paymentDoc.paths)
            {
                if (billingDoc.paths[path.Name] == null)
                {
                    billingDoc.paths[path.Name] = path.Value;
                }
                else
                {
                    billingDoc.paths[$"/payment{path.Name}"] = path.Value; // Prefix to avoid conflicts
                }
            }

            // Merge schemas if they exist
            if (paymentDoc.components != null && paymentDoc.components.schemas != null)
            {
                foreach (var schema in paymentDoc.components.schemas)
                {
                    if (billingDoc.components.schemas[schema.Name] == null)
                    {
                        billingDoc.components.schemas[schema.Name] = schema.Value;
                    }
                    else
                    {
                        billingDoc.components.schemas[$"Payment_{schema.Name}"] = schema.Value; // Prefix to avoid conflicts
                    }
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(billingDoc);
        }
    }
}
