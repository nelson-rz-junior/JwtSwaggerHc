using JwtSwaggerHc.API.Authorization;
using JwtSwaggerHc.API.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace JwtSwaggerHc.API.V2.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public UserController(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        /// <summary>
        /// Get user content
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUser")]
        [Authorize(Roles = Policies.User)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUserData()
        {
            var result = "This is a response from USER method";

            await new NotificationHub().SendMessage(_hubContext, "messageReceived", "GetUserData()", $"{result}");

            return Ok(result);
        }

        /// <summary>
        /// Get manager content
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetManager")]
        [Authorize(Roles = Policies.Manager)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetManagerData()
        {
            var result = "This is a response from MANAGER method";

            await new NotificationHub().SendMessage(_hubContext, "messageReceived", "GetManagerData()", $"{result}");

            return Ok(result);
        }

        /// <summary>
        /// Get admin content
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAdmin")]
        [Authorize(Roles = Policies.Admin)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAdminData()
        {
            var result = "This is a response from ADMIN method";

            await new NotificationHub().SendMessage(_hubContext, "messageReceived", "GetAdminData()", $"{result}");

            return Ok(result);
        }
    }
}
