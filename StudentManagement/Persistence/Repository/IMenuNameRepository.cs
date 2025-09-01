using StudentManagement.Persistence.Model;

namespace StudentManagement.Persistence.Repository
{
    public interface IMenuNameRepository
    {
        Task AddAsync(MenuName menuName);
        Task DeleteAsync(int menuId);
        Task<List<MenuName>> GetAllAsync();
        Task<MenuName?> GetByIdAsync(int menuId);
        Task UpdateAsync(MenuName menuName);
    }
}