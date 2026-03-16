using JwtDotnet8Prectice.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtDotnet8Prectice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthLoginController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public AuthLoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private readonly List<User> _users = new List<User>()
        {
            new User {Id = 1, Email ="Alice@Example.com", Name = "Alice", Password = "alice123", Roles = "Manager" },
            new User {Id = 2, Email ="Bob@Example.com", Name = "Bob", Password = "bob123", Roles = "User" },
            new User {Id = 3, Email ="Charlie@Example.com", Name = "Charlie", Password = "charlie123", Roles = "Admin" }
        };
        [HttpPost("Login")]
        [ResponseCache(Duration = 505)]
        public async Task<ActionResult> Login(UserLogin obj)
        {
            if (obj == null)
            {
                return BadRequest();
            }
            var data = _users.FirstOrDefault(ex => ex.Email.ToLower() == obj.Username.ToLower() && ex.Password.ToLower() == obj.Password.ToLower());
            if (data == null)
            {
                return NotFound("User Not Found");
            }
            var getJwtsection = _configuration.GetSection("JwtSection");
            var getkey = getJwtsection.GetSection("SecretKey").Value;
            var getIssuer = getJwtsection.GetSection("Issuer").Value;
            var getAudience = getJwtsection.GetSection("autdience").Value;
            var securekey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(getkey));

            var descriptor = new SecurityTokenDescriptor
            {
                Audience = getAudience,
                Expires = DateTime.UtcNow.AddMinutes(35),
                Issuer = getIssuer,
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserId", data.Id.ToString()),
                    new Claim("UserName" ,data.Name),
                    new Claim(ClaimTypes.Role ,data.Roles)
                }),
                SigningCredentials  = new SigningCredentials(securekey,SecurityAlgorithms.HmacSha256)
            };
            var jwtHanlder = new JwtSecurityTokenHandler();
            var CreateJwtToken = jwtHanlder.CreateToken(descriptor);
            var mainToken = jwtHanlder.WriteToken(CreateJwtToken);
            
            return Ok( new { Token = mainToken ,UserName = data.Name});
        }
    }
}
