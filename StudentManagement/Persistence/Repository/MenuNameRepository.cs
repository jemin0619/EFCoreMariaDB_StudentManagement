using Microsoft.EntityFrameworkCore;
using StudentManagement.Persistence.DataBaseContext;
using StudentManagement.Persistence.Model;

namespace StudentManagement.Persistence.Repository
{
    public class MenuNameRepository : IMenuNameRepository
    {
        private readonly MenuDbContext _context;

        public MenuNameRepository(MenuDbContext context)
        {
            _context = context;
        }

        public async Task<List<MenuName>> GetAllAsync()
            => await _context.MenuNames.ToListAsync();

        public async Task<MenuName?> GetByIdAsync(int menuId)
            => await _context.MenuNames.FindAsync(menuId);

        public async Task AddAsync(MenuName menuName)
        {
            _context.MenuNames.Add(menuName);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MenuName menuName)
        {
            _context.MenuNames.Update(menuName);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int menuId)
        {
            var menuName = await _context.MenuNames.FindAsync(menuId);
            if (menuName != null)
            {
                _context.MenuNames.Remove(menuName);
                await _context.SaveChangesAsync();
            }
        }
    }

}
