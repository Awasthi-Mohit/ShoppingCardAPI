using ApiForAng.ApplicationDbcontext;
using ApiForAng.DTO;
using ApiForAng.Models;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiForAng.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public LoginController(IConfiguration configuration,ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDto request)
        {
            try
            {
                if (request == null)
                    return BadRequest("Invalid request");
 var user = _context.uses.FirstOrDefault(u => u.Email == request.Email && u.Password == request.Password);
                //             bool isPasswordValid = BCrypt.Net.BCrypt.Verify(
                //    request.Password,
                //    user.PasswordHash
                //);

                //if (!isPasswordValid)
                //    return Unauthorized("Invalid email or password");
                string role = user.Email.ToLower() == "awasthi221@gmail.com" ? "Admin" : "User";

                var token = GenerateJwtToken(request.Email);

                return Ok(new
                {
                    token,
                    email = request.Email
                });

                return Unauthorized("Invalid email or password");
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
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
        [Authorize]
        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            return Ok("JWT working 🚀");
        }



        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserDTO dto)
        {
            try
            {
                if(ModelState.IsValid)
                {

                    var user = new User
                    {
                        Name = dto.Name,
                        Email = dto.Email,
                        Password= dto.Password,
                        Address = dto.Address,
                        City = dto.City,
                      Number = dto.Number
                    };
                    _context.uses.Add(user);
                    await _context.SaveChangesAsync();
                    return Ok("User registered successfully");
                }
                else
                {
                    return BadRequest("Invalid user data");
                }
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }
}
