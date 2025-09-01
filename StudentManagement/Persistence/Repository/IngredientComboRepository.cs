using Microsoft.EntityFrameworkCore;
using StudentManagement.Persistence.DataBaseContext;
using StudentManagement.Persistence.Model;

namespace StudentManagement.Persistence.Repository
{
    public class IngredientComboRepository : IIngredientComboRepository
    {
        private readonly MenuDbContext _context;

        public IngredientComboRepository(MenuDbContext context)
        {
            _context = context;
        }

        public async Task<List<IngredientCombo>> GetAllAsync()
            => await _context.IngredientCombos.ToListAsync();

        public async Task<IngredientCombo?> GetByIdAsync(int ingredientComboId)
            => await _context.IngredientCombos.FindAsync(ingredientComboId);

        public async Task AddAsync(IngredientCombo combo)
        {
            _context.IngredientCombos.Add(combo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(IngredientCombo combo)
        {
            _context.IngredientCombos.Update(combo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int ingredientComboId)
        {
            var combo = await _context.IngredientCombos.FindAsync(ingredientComboId);
            if (combo != null)
            {
                _context.IngredientCombos.Remove(combo);
                await _context.SaveChangesAsync();
            }
        }
    }

}
