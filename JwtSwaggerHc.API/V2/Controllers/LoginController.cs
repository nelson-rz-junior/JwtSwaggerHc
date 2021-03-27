using JwtSwaggerHc.API.Authorization;
using JwtSwaggerHc.API.V2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace JwtSwaggerHc.API.V2.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private List<User> users = new List<User>
        {
            new User { Username = "admin1", Password = "1234", Role = Policies.Admin },
            new User { Username = "manager1", Password = "1234", Role = Policies.Manager },
            new User { Username = "user1", Password = "1234", Role = Policies.User }
        };

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        ///  Creates a token using username and password
        /// </summary>
        /// <param name="loginRequest">Request input</param>
        /// <returns>Token and user informations</returns>
        [HttpPost]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult Login(LoginRequest loginRequest)
        {
            User user = AuthenticateUser(loginRequest);
            if (user == null)
            {
                return Unauthorized();
            }

            var token = GenerateJwt(user);

            return Ok(new LoginResponse
            {
                Token = token,
                User = user
            });
        }

        /// <summary>
        ///  Retrieve token credentials
        /// </summary>
        /// <param name="token">Request token</param>
        /// <returns>Credential information</returns>
        [HttpPost("Token/Claims")]
        [ProducesResponseType(typeof(ClaimsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetClaims(string token)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = securityKey
            };

            try
            {
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

                var result = new ClaimsResponse
                {
                    Name = claimsPrincipal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value,
                    Role = claimsPrincipal.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Value,
                    Jti = claimsPrincipal.FindFirst("jti").Value
                };

                return Ok(new JsonResult(result));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Invalid token: {ex.Message}");
            }
        }

        private User AuthenticateUser(LoginRequest userLogin)
        {
            return users.FirstOrDefault(u => u.Username == userLogin.Username && u.Password == userLogin.Password);
        }

        private string GenerateJwt(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha384);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
