using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Azure.Core;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using project_api_qlsv_NH.Entities;
using project_api_qlsv_NH.Dtos;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace project_api_qlsv_NH.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : Controller
    {
        private readonly StudentDBcontext _context;
        private readonly IConfiguration _configuration;

        public StudentController(StudentDBcontext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [HttpGet]
        [Route("getAll")]
        //public IActionResult Index(int? limit, int? page)
        public IActionResult Index()
        {
            var students = _context.Students
                .OrderBy("p => p.studentId DESC")
                .ToArray();
            if (students ==  null)
            {
                Console.WriteLine("Don't student");

            }
            return Ok(students);

        }

        [HttpGet]
        [Route("getAllAD")]
        [Authorize(Policy = "Admin")]
        //public IActionResult Index(int? limit, int? page)
        public IActionResult IndexAD()
        {
            var students = _context.Students
                .OrderBy("p => p.studentId DESC")
                .ToArray();
            if (students == null)
            {
                Console.WriteLine("Don't student");

            }
            return Ok(students);

        }


        [HttpPost]
        [Route("create")]
        [Authorize(Policy = "Admin")]
        public IActionResult CreateAStudent(Student s)
        {
            var userExists = _context.Users.Any(u => u.UserId == s.UserId);
            if (!userExists)
            {
                return BadRequest("Invalid UserId");
            }
            // Thêm sinh viên
            _context.Students.Add(s);
            _context.SaveChanges();
            return Created($"/get-by-id?id={s.StudentId}", s);
        }



        [HttpPut]
        [Route("update")]
        [Authorize(Policy = "Admin")]
        public IActionResult UpdateStudent(int id, [FromBody] Student updatedStudent)
        {
            var u = _context.Students.Find(id);

            if (u == null)
            {
                return NotFound();
            }
            // Cập nhật các thuộc tính của sinh viên với dữ liệu mới
            u.FullName = updatedStudent.FullName;
            u.DateOfBirth = updatedStudent.DateOfBirth;
            u.Address = updatedStudent.Address;
            u.Email = updatedStudent.Email;
            u.ImageUrl = updatedStudent.ImageUrl;

            // Lưu thay đổi vào database
            _context.SaveChanges();

            return Ok();
        }


        [HttpDelete]
        [Route("delete")]
        [Authorize(Policy = "Admin")]
        public IActionResult DeleteStudent(int id)
        {
            //Tìm student dựa trên id
            var s = _context.Students.Find(id);

            if (s == null)
            {
                return NotFound();
            }
            //Xóa product khỏi databasse
            _context.Students.Remove(s);
            _context.SaveChanges();

            return Ok("This student has been removed");
        }



        [HttpGet]
        [Route("get-by-id")]
        public IActionResult GetSingleStudent(int id)
        {
            //Truy vấn sinh viên dựa trệ id 
            var student = _context.Students
                .Where(p => p.UserId == id)
                .First();
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }




    }
}

