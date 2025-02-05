using Notification.Application;
using Notification.Domain;
using Notification.Infrastructure;

namespace Norification.Application
{
    public class NotificationService : INotificationService
    {
        private readonly IFcmTokenRepository _repository;

        public NotificationService(IFcmTokenRepository repository)
        {
            _repository = repository;
        }

        public async Task RegisterTokenAsync(int userId, string token)
        {
            // Check if the token already exists
            var existingToken = await _repository.GetTokenAsync(userId, token);
            if (existingToken != null)
            {
                return; // Token already registered
            }

            // Save the new token
            var fcmToken = new FcmToken
            {
                
                UserId = userId,
                Token = token
            };

            await _repository.SaveTokenAsync(fcmToken);
        }
    }
}
