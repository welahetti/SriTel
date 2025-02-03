namespace Customers.Application.DTO
{
    public class UpdateProfileRequest
    {
        public Guid CustomerId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
