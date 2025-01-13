using Billing.Service.Billing.Domain;

namespace Billing.Service.Billing.Application.ExternalServices
{
    public class CustomerServiceClient
    {
        private readonly HttpClient _httpClient;

        public CustomerServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            var response = await _httpClient.GetAsync($"https://customer-service/api/users/{userId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<User>();
        }
    }
}


