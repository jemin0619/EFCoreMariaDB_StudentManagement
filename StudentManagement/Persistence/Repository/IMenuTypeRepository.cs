using StudentManagement.Persistence.Model;

namespace StudentManagement.Persistence.Repository
{
    public interface IMenuTypeRepository
    {
        Task AddAsync(MenuType menuType);
        Task DeleteAsync(int menuTypeId);
        Task<List<MenuType>> GetAllAsync();
        Task<MenuType?> GetByIdAsync(int menuTypeId);
        Task UpdateAsync(MenuType menuType);
    }
}