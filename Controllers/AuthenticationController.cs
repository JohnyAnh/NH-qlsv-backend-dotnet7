using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using project_api_qlsv_NH.Dtos;
using BCrypt.Net;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace project_api_qlsv_NH.Controllers
{
    [ApiController]
    [Route("/api/auth")]
    public class AuthenticationController : Controller
    {
        // GET: /<controller>/
        private readonly StudentDBcontext _context;
        private readonly IConfiguration _config;
        public AuthenticationController(StudentDBcontext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(UserRegister userRequestDto)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Username == userRequestDto.Username);
            if (existingUser != null)
            {
                return BadRequest("Username is already registered");
            }
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(userRequestDto.PasswordHash);

            var u = new Entities.User
            {
                Username = userRequestDto.Username,
                PasswordHash = passwordHash,
                Role = userRequestDto.Role,
                Status = userRequestDto.Status
            };

            _context.Users.Add(u);
            _context.SaveChanges();
            return Ok(new UserData { Id = u.UserId, Name = u.Username, Role = u.Role , Token = GenerateJWT(u) });
        }
        private String GenerateJWT(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var signatureKey = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.Role,user.Role),
            };
            var token = new JwtSecurityToken(
                _config["JWT:Issuer"],
                _config["JWT:Audience"],
                claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: signatureKey
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login(UserLogin userLogin) { 

            try {
                var user = _context.Users.FirstOrDefault(u => u.Username.Equals(userLogin.Username));

                if (user == null)
                {
                    return Unauthorized("Username not found");
                }

                bool verified = BCrypt.Net.BCrypt.Verify(userLogin.Password, user.PasswordHash);

                if (!verified)
                {
                    return Unauthorized("Password is not true.");
                }

                // Trả về thông tin người dùng cùg với mã JWT (Token)
                return Ok(new UserData { Id = user.UserId, Name = user.Username, Role = user.Role, Token = GenerateJWT(user) });
            } catch (Exception ex) {
                //file.wrie(ex.message)
                return BadRequest();
            }
            
        }




    }
}

