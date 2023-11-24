using System;
using System.ComponentModel.DataAnnotations;

namespace project_api_qlsv_NH.Dtos
{
	public class UserRegister
	{
        public int UserId { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? PasswordHash { get; set; }
        [Required]
        public string? Role { get; set; }
        [Required]
        public int Status { get; set; }
    }
}

