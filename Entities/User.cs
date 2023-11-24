using System.Collections.Generic;

namespace project_api_qlsv_NH.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public string? Role { get; set; }
        public int Status { get; set; }

        // Tham chiếu đến danh sách sinh viên của người dùng
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}



