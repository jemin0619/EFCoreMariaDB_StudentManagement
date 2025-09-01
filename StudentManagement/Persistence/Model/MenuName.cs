using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace StudentManagement.Persistence.Model
{
    public class MenuName
    {
        [Key]
        public int MenuId { get; set; }
        public string MenuNameValue { get; set; } = string.Empty;
        public virtual ICollection<Menu> Menus { get; set; }

    }
}
