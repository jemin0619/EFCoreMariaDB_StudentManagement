using StudentManagement.Persistence.Model;

namespace StudentManagement.Persistence.Repository
{
    public interface IMenuRepository
    {
        Task AddAsync(Menu menu);
        Task DeleteAsync(int menuId, int menuTypeId);
        Task<List<Menu>> GetAllAsync();
        Task<Menu?> GetByIdAsync(int menuId, int menuTypeId);
        Task UpdateAsync(Menu menu);
    }
}