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
                var customerServiceUrl = _configuration["Services:Customer"];
                var client = _httpClientFactory.CreateClient();

                try
                {
                    var customerSwagger = await client.GetStringAsync($"{customerServiceUrl}/swagger/v1/swagger.json");
                    var billingSwagger = await client.GetStringAsync($"{billingServiceUrl}/swagger/v1/swagger.json");
                    var paymentSwagger = await client.GetStringAsync($"{paymentServiceUrl}/swagger/v1/swagger.json");


                    var mergedSwagger = MergeSwaggerDocuments(customerSwagger,billingSwagger, paymentSwagger);

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
        private string MergeSwaggerDocuments(string customerSwagger, string billingSwagger, string paymentSwagger)
        {
            // Deserialize all Swagger documents
            dynamic customerDoc = Newtonsoft.Json.JsonConvert.DeserializeObject(customerSwagger);
            dynamic billingDoc = Newtonsoft.Json.JsonConvert.DeserializeObject(billingSwagger);
            dynamic paymentDoc = Newtonsoft.Json.JsonConvert.DeserializeObject(paymentSwagger);

            // Merge customer paths into billingDoc
            foreach (var path in customerDoc.paths)
            {
                if (billingDoc.paths[path.Name] == null)
                {
                    billingDoc.paths[path.Name] = path.Value;
                }
                else
                {
                    billingDoc.paths[$"/customer{path.Name}"] = path.Value; // Prefix to avoid conflicts
                }
            }

            // Merge payment paths into billingDoc
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

            // Merge customer schemas into billingDoc
            if (customerDoc.components != null && customerDoc.components.schemas != null)
            {
                foreach (var schema in customerDoc.components.schemas)
                {
                    if (billingDoc.components.schemas[schema.Name] == null)
                    {
                        billingDoc.components.schemas[schema.Name] = schema.Value;
                    }
                    else
                    {
                        billingDoc.components.schemas[$"Customer_{schema.Name}"] = schema.Value; // Prefix to avoid conflicts
                    }
                }
            }

            // Merge payment schemas into billingDoc
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

            // Return the merged Swagger document as a JSON string
            return Newtonsoft.Json.JsonConvert.SerializeObject(billingDoc);
        }

    }
}
