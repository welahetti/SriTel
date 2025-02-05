using Microsoft.AspNetCore.Mvc;
using Notification.Application;
using Notification.Application.DTO;
using System.Net.Http.Headers;

namespace Notification.API
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly INotificationService _notificationService;

        public NotificationsController(IConfiguration configuration,INotificationService notificationService)
        {
            _configuration = configuration;
            _notificationService = notificationService;
        }

        [HttpPost("register")]
        public IActionResult RegisterToken([FromBody] FcmTokenDto tokenDto)
        {
            // Save the token to the database (pseudo-code)
            _notificationService.RegisterTokenAsync(tokenDto.UserId, tokenDto.Token);

            return Ok(new { Message = "Token registered successfully" });
        }
    

        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
        {
            var fcmServerKey = _configuration["Firebase:ServerKey"];
            var fcmUrl = "https://fcm.googleapis.com/fcm/send";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("key", "=" + fcmServerKey);

                var payload = new
                {
                    to = request.Token, // Recipient token
                    notification = new
                    {
                        title = request.Title,
                        body = request.Body
                    },
                    data = request.Data // Custom data payload
                };

                var response = await client.PostAsJsonAsync(fcmUrl, payload);
                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { Message = "Notification sent successfully" });
                }

                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }


    }

}
