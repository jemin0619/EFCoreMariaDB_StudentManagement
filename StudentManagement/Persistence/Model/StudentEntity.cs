using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Persistence.Model
{
    public class StudentEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public int Age { get; set; }
    }
}
