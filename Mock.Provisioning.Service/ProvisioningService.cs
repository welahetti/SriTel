namespace Mock.Provisioning.Service
{
    public class ProvisioningService : IProvisioningService
    {
        public async Task<ProvisioningResult> ActivateServiceAsync(ServiceRequest serviceRequest)
        {
            // Simulate some activation logic
            await Task.Delay(500); // Simulate network delay
            return new ProvisioningResult { IsSuccess = true };
        }

        public async Task<ProvisioningResult> DeactivateServiceAsync(ServiceRequest serviceRequest)
        {
            // Simulate some deactivation logic
            await Task.Delay(500); // Simulate network delay
            return new ProvisioningResult { IsSuccess = true };
        }
    }

}
