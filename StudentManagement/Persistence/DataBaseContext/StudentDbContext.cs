using Microsoft.EntityFrameworkCore;
using StudentManagement.Persistence.Model;

namespace StudentManagement.Persistence.DataBaseContext
{
    public class StudentDbContext : DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options)
        {
            
        }

        public DbSet<StudentEntity> Students { get; set; }
    }
}
