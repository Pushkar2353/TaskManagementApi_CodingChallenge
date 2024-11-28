using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagementApi_CodingChallenge.Models;

namespace TaskManagementApi_CodingChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] User user)
        {
            _logger.LogInformation("Login attempt for user: {Username}", user.Username);

            if (user.Username == "Admin" && user.Password == "Password@123")
            {
                var token = GenerateJwtToken(user.Username);
                _logger.LogInformation("Login successful for user: {Username}", user.Username);
                return Ok(new { Token = token });
            }

            _logger.LogWarning("Login failed for user: {Username}", user.Username);
            return Unauthorized("Invalid credentials");
        }

        private string GenerateJwtToken(string username)
        {
            _logger.LogInformation("Generating JWT token for user: {Username}", username);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KHPK6Ucf/zjvU4qW8/vkuuGLHeIo0l9ACJiTaAPLKbk="));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: "TMIssuer",
                audience: "TMAudience",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            _logger.LogInformation("JWT token successfully generated for user: {Username}", username);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
