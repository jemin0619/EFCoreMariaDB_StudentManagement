using Microsoft.EntityFrameworkCore;
using StudentManagement.Persistence.DataBaseContext;
using StudentManagement.Persistence.Model;

namespace StudentManagement.Persistence.Repository
{
    public class MenuTypeRepository : IMenuTypeRepository
    {
        private readonly MenuDbContext _context;

        public MenuTypeRepository(MenuDbContext context)
        {
            _context = context;
        }

        public async Task<List<MenuType>> GetAllAsync()
            => await _context.MenuTypes.ToListAsync();

        public async Task<MenuType?> GetByIdAsync(int menuTypeId)
            => await _context.MenuTypes.FindAsync(menuTypeId);

        public async Task AddAsync(MenuType menuType)
        {
            _context.MenuTypes.Add(menuType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MenuType menuType)
        {
            _context.MenuTypes.Update(menuType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int menuTypeId)
        {
            var menuType = await _context.MenuTypes.FindAsync(menuTypeId);
            if (menuType != null)
            {
                _context.MenuTypes.Remove(menuType);
                await _context.SaveChangesAsync();
            }
        }
    }

}
