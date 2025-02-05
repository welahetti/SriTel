namespace Notification.Application
{
    public interface INotificationService
    {
        Task RegisterTokenAsync(int userId, string token);
    }
}
