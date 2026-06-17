using ApiForAng.ApplicationDbcontext;
using ApiForAng.DTO;
using ApiForAng.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiForAng.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthController(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto request)
        {
            if (request == null)
                return BadRequest("Invalid request");

            var user = _context.uses
                .FirstOrDefault(u => u.Email == request.Email
                                  && u.Password == request.Password);


            if (user == null)
                return Unauthorized("Invalid email or password");

            var token = GenerateJwtToken(user.Email);

            bool isAdmin = string.Equals(user.Email, "awasthi221@gmail.com", StringComparison.OrdinalIgnoreCase);

            return Ok(new
            {
                token,
                email = user.Email,
                role = isAdmin ? "Admin" : "User" 
            });
        }

        // ✅ REGISTER
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid user data");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password,
                Address = dto.Address,
                City = dto.City,
                Number = dto.Number
            };

            _context.uses.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully");
        }

        // ✅ PROTECTED ROUTE
        [Authorize]
        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            return Ok("JWT working 🚀");
        }

        private string GenerateJwtToken(string email)
        {
            var jwt = _configuration.GetSection("Jwt");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToInt32(jwt["ExpireMinutes"])
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
