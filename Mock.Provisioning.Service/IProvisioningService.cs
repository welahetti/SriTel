namespace Mock.Provisioning.Service
{
    public interface IProvisioningService
    {
        Task<ProvisioningResult> ActivateServiceAsync(ServiceRequest serviceRequest);
        Task<ProvisioningResult> DeactivateServiceAsync(ServiceRequest serviceRequest);
    }
}
