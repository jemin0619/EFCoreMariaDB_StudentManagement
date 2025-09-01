using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Persistence.Model
{
    public class IngredientCombo
    {
        [Key]
        public int IngredientComboId { get; set; }
        public int Ingredient1 { get; set; }
        public int Ingredient2 { get; set; }
        public int Ingredient3 { get; set; }

        public virtual ICollection<Menu> Menus { get; set; }
    }
}
