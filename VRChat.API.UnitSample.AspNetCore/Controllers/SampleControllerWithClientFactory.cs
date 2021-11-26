using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using VRChat.API.Client;
using VRChat.API.Extensions.Hosting;

namespace VRChat.API.UnitSample.AspNetCore.Controllers
{
    [ApiController]
    public class SampleControllerWithClientFactory : ControllerBase
    {
        private readonly IVRChat _vrchat;
        private readonly ILogger _logger;

        public SampleControllerWithClientFactory(ILogger<SampleControllerWithClientFactory> logger, IVRChatClientFactory vrcFactory)
        {
            _vrchat = vrcFactory.CreateClient("OtherUser");
            _logger = logger;
        }

        [HttpGet("/{id}")]
        public async Task<IActionResult> GetCurrentUserAsync(string userId)
        {
            if (!VRCGuid.TryParse(userId, out VRCGuid id))
                return BadRequest(new { error = "VRChat user ID was not formatted correctly." });

            if (id.Kind != VRCKind.User)
                return BadRequest(new { error = "This endpoint can only fetch users!" });

            var user = await _vrchat.Users.GetUserAsync(id.ToString());
            _logger.LogInformation("IP address '{ip}' requested user: {userId}, {username}.",
                    HttpContext.Connection.RemoteIpAddress.ToString(),
                    id.ToString(),
                    user.Username
                );

            return Ok(new
            {
                id,
                user.Username,
                user.DisplayName,
                user.Bio,
                user.CurrentAvatarImageUrl
            });
        }
    }
}
