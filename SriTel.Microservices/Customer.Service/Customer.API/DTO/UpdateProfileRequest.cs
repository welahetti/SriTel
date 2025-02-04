namespace Customer.Service.Customer.API.DTO
{
    public class UpdateProfileRequest
    {
        public Guid CustomerId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
