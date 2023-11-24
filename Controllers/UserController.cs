using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using project_api_qlsv_NH.Entities;
using System.Linq.Dynamic.Core;
using System.Security.Claims;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace project_api_qlsv_NH.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly StudentDBcontext _context;
        private readonly IConfiguration _configuration;
        public UserController(StudentDBcontext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("getAll")]
        [Authorize(Policy = "Admin")]
        public IActionResult Index()
        {
            var users = _context.Users
                .OrderBy("p => p.userId DESC")
                .ToArray();
            if (users == null)
            {
                Console.WriteLine("Don't user");

            }
            return Ok(users);

        }

        [HttpDelete]
        [Route("delete")]
        [Authorize(Policy = "Admin")]
        public IActionResult DeleteAUser(int id)
        {

            // Lấy thông tin người dùng hiện tại từ Claims
            var currentUserId = GetCurrentUserIdFromClaims();
            //Tìm user dựa trên id
            var u = _context.Users.Find(id);

            if (u == null)
            {
                return NotFound();
            }
            // Kiểm tra nếu người dùng có vai trò là Admin và không được phép tự xóa
            if (User.IsInRole("Admin") && u.UserId.ToString() == currentUserId)
            {
                return BadRequest("Admin cannot delete itself");
            }

            u.Status = 1;

            _context.SaveChanges();

            return Ok("This user has been removed");
        }

        private object GetCurrentUserIdFromClaims()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim != null)
            {
                return userIdClaim.Value;
            }

            return null;
        }
    }

}

