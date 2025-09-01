using Microsoft.EntityFrameworkCore;
using StudentManagement.Persistence.DataBaseContext;
using StudentManagement.Persistence.Model;

namespace StudentManagement.Persistence.Repository
{
    public class MenuRepository : IMenuRepository
    {
        private readonly MenuDbContext _context;

        public MenuRepository(MenuDbContext context)
        {
            _context = context;
        }

        public async Task<List<Menu>> GetAllAsync()
        {
            return await _context.Menus
                .Include(m => m.MenuType) // 메뉴 유형 포함
                .Include(m => m.MenuName) // 메뉴 이름 포함
                .Include(m => m.IngredientCombo) // 재료 조합 포함
                .ToListAsync();
        }

        public async Task<Menu?> GetByIdAsync(int menuId, int menuTypeId)
            => await _context.Menus.FindAsync(menuId, menuTypeId);

        public async Task AddAsync(Menu menu)
        {
            _context.Menus.Add(menu);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Menu menu)
        {
            _context.Menus.Update(menu);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int menuId, int menuTypeId)
        {
            var menu = await _context.Menus.FindAsync(menuId, menuTypeId);
            if (menu != null)
            {
                _context.Menus.Remove(menu);
                await _context.SaveChangesAsync();
            }
        }
    }

}
