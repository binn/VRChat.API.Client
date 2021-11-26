using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using VRChat.API.Client;

namespace VRChat.API.UnitSample.AspNetCore.Controllers
{
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly IVRChat _vrchat;
        private readonly ILogger _logger;

        public SampleController(ILogger<SampleController> logger, IVRChat vrchat)
        {
            _vrchat = vrchat;
            _logger = logger;
        }

        [HttpGet("/{userId}")]
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
                user.Id,
                user.Username,
                user.DisplayName,
                user.Bio,
                user.CurrentAvatarImageUrl
            });
        }
    }
}
