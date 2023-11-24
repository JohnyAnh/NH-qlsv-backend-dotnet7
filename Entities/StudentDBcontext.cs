//StudentDBcontext.cs
using Microsoft.EntityFrameworkCore;
using System;
using project_api_qlsv_NH.Entities;

namespace project_api_qlsv_NH.Entities
{
    public class StudentDBcontext : DbContext
    {
        public static String connectionString;

        public StudentDBcontext()
        {
        }

        public StudentDBcontext(DbContextOptions<StudentDBcontext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=localhost,1433; Database=quanlysinhvienNH;User Id=sa;Password=Password.1;TrustServerCertificate=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình mối quan hệ nhiều-một giữa User và Student
            modelBuilder.Entity<User>()
                .HasMany(u => u.Students)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade); 

            
        }
    }
}
