using Notification.Domain;

namespace Notification.Infrastructure
{
    public interface IFcmTokenRepository
    {
        Task SaveTokenAsync(FcmToken token);
        Task<FcmToken?> GetTokenAsync(int userId, string token);
    }


}
