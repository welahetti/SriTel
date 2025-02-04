﻿using Microsoft.AspNetCore.Http;
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
                    // Fetch Swagger JSONs
                    var customerSwagger = await client.GetStringAsync($"{customerServiceUrl}/swagger/v1/swagger.json");
                    var billingSwagger = await client.GetStringAsync($"{billingServiceUrl}/swagger/v1/swagger.json");
                    var paymentSwagger = await client.GetStringAsync($"{paymentServiceUrl}/swagger/v1/swagger.json");

                    // Merge Swagger Documents
                    var mergedSwagger = MergeSwaggerDocuments(customerSwagger, billingSwagger, paymentSwagger);

                    // Write Merged Document
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
            dynamic customerDoc = Newtonsoft.Json.JsonConvert.DeserializeObject(customerSwagger);
            dynamic billingDoc = Newtonsoft.Json.JsonConvert.DeserializeObject(billingSwagger);
            dynamic paymentDoc = Newtonsoft.Json.JsonConvert.DeserializeObject(paymentSwagger);

            // Merge Paths
            MergePaths(customerDoc, billingDoc, "customer");
            MergePaths(paymentDoc, billingDoc, "payment");

            // Merge Schemas
            MergeSchemas(customerDoc, billingDoc, "Customer");
            MergeSchemas(paymentDoc, billingDoc, "Payment");

            return Newtonsoft.Json.JsonConvert.SerializeObject(billingDoc);
        }

        private void MergePaths(dynamic sourceDoc, dynamic targetDoc, string prefix)
        {
            if (sourceDoc?.paths == null) return;

            foreach (var path in sourceDoc.paths)
            {
                if (targetDoc.paths[path.Name] == null)
                {
                    targetDoc.paths[path.Name] = path.Value;
                }
                else
                {
                    targetDoc.paths[$"/{prefix}{path.Name}"] = path.Value; // Avoid conflicts
                }
            }
        }

        private void MergeSchemas(dynamic sourceDoc, dynamic targetDoc, string prefix)
        {
            if (sourceDoc?.components?.schemas == null) return;

            foreach (var schema in sourceDoc.components.schemas)
            {
                if (targetDoc.components.schemas[schema.Name] == null)
                {
                    targetDoc.components.schemas[schema.Name] = schema.Value;
                }
                else
                {
                    targetDoc.components.schemas[$"{prefix}_{schema.Name}"] = schema.Value; // Avoid conflicts
                }
            }
        }
    }
}
