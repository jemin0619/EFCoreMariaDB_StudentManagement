using StudentManagement.Persistence.Model;

namespace StudentManagement.Persistence.Repository
{
    public interface IIngredientComboRepository
    {
        Task AddAsync(IngredientCombo combo);
        Task DeleteAsync(int ingredientComboId);
        Task<List<IngredientCombo>> GetAllAsync();
        Task<IngredientCombo?> GetByIdAsync(int ingredientComboId);
        Task UpdateAsync(IngredientCombo combo);
    }
}