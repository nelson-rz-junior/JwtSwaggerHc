using JwtSwaggerHc.API.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtSwaggerHc.API.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// Get user content
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get")]
        [Authorize(Roles = Policies.User)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetUserData()
        {
            return Ok("This is a response from USER method");
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
        public IActionResult GetAdminData()
        {
            return Ok("This is a response from ADMIN method");
        }
    }
}
