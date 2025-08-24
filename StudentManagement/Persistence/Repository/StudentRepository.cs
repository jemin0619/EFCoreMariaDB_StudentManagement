using StudentManagement.Persistence.DataBaseContext;
using StudentManagement.Persistence.Model;

namespace StudentManagement.Persistence.Repository
{
    public class StudentRepository : GenericRepository<StudentEntity>, IStudentRepository
    {
        public StudentRepository(StudentDbContext context) : base(context)
        {
        }
    }
}
