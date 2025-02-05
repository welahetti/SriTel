using Notification.Domain;
using Notification.Infrastructure;

namespace Norification.Infrastructure
{
    public class FcmTokenRepository : IFcmTokenRepository
    {
        private readonly NotificationDbContext _dbContext;

        public FcmTokenRepository(NotificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<FcmToken?> GetTokenAsync(int userId, string token)
        {
            throw new NotImplementedException();
        }

        public async Task SaveTokenAsync(FcmToken token)
        {
            //_dbContext.FcmTokens.Add(token);
            await _dbContext.SaveChangesAsync();
        }

        
    }
}
