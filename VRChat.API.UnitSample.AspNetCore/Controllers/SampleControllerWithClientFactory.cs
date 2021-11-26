using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
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

        [HttpGet("/worlds/trending")]
        public async Task<IActionResult> GetTrendingWorldsAsync()
        {
            var worlds = await _vrchat.Worlds.GetActiveWorldsAsync();
            _logger.LogInformation("IP address '{ip}' requested trending worlds, {worldCount} was received.",
                    HttpContext.Connection.RemoteIpAddress.ToString(),
                    worlds.Count
                );

            return Ok(worlds.Select(world => new 
            {
                world.Id,
                world.Name,
                world.Heat,
                world.ImageUrl,
                world.AuthorName,
                world.AuthorId,
                world.Capacity,
                world.Favorites,
                world.Occupants
            }));
        }
    }
}
