using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TodoApp.Application.DTOs;
using System.Text;

namespace TodoApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        #region Methods
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto model)
        {
            if (IsValidUser(model))
            {
                var token = GenerateJwtToken(model.UserName);
                return Ok(new { AccessToken = token });
            }
            return Unauthorized();
        }

        private string GenerateJwtToken(string username)
        {
            var claims = new[]
            {
                    new Claim(ClaimTypes.Name, username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-very-secure-128-bit-secret-key"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(              
                claims: claims,
                expires: DateTime.Now.AddDays(10),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion

        #region Helper
        private bool IsValidUser(LoginDto model)
        {
            return model.UserName == "testuser" && model.Password == "123";
        }
        #endregion
    }
}
