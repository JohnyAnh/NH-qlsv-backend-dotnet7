using System;

namespace project_api_qlsv_NH.Entities
{
    public class Student
    {
        public int StudentId { get; set; }
        public string? FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? ImageUrl { get; set; }
        public string? Email { get; set; }

        // Khóa ngoại tham chiếu đến Users
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}

