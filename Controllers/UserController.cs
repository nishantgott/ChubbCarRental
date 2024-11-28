using Microsoft.AspNetCore.Mvc;
using ChubbCarRental.Models;
using ChubbCarRental.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;

namespace ChubbCarRental.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepositary _userRepositary;
        private readonly IConfiguration _configuration;

        public UserController(IUserRepositary userRepositary, IConfiguration configuration)
        {
            _userRepositary = userRepositary;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserModel user)
        {
            if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Name, Email, and Password are required.");
            }

            var existingUser = _userRepositary.GetUserByEmail(user.Email);
            if (existingUser != null)
            {
                return Conflict("User with the same email already exists.");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _userRepositary.AddUser(user);
            return CreatedAtAction(nameof(Register), new { id = user.Id }, user);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserModel login)
        {
            if (string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest("Email and Password are required.");
            }

            var user = _userRepositary.GetUserByEmail(login.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(UserModel user)
        {
            var key = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(key) || Encoding.UTF8.GetBytes(key).Length < 32)
            {
                throw new ArgumentException("JWT Key must be at least 32 characters long.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
