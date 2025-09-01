using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Persistence.Model
{
    public class MenuType
    {
        [Key]
        public int MenuTypeId { get; set; }
        public string MenuTypeName { get; set; } = string.Empty;
        public virtual ICollection<Menu> Menus { get; set; }
    }
}
