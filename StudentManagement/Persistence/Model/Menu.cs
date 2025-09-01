using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Persistence.Model
{
    public class Menu
    {
        //[Key, Column(Order = 0)]
        [ForeignKey("MenuName")]
        public int MenuId { get; set; }

        //[Key, Column(Order = 1)]
        [ForeignKey("MenuType")]
        public int MenuTypeId { get; set; }

        [ForeignKey("IngredientCombo")]
        public int IngredientComboId { get; set; }

        public virtual MenuName MenuName { get; set; }
        public virtual MenuType MenuType { get; set; }
        public virtual IngredientCombo IngredientCombo { get; set; }
    }
}
