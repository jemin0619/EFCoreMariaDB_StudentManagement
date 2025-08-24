
using Microsoft.EntityFrameworkCore;
using StudentManagement.Persistence.DataBaseContext;
using StudentManagement.Persistence.Model;

namespace StudentManagement.Persistence.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StudentDbContext _context;
        public GenericRepository(StudentDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(T entity)
        {
            entity.DateCreated = DateTime.Now;
            entity.DateModified = DateTime.Now;
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync()
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
            
        }

        public async Task UpdateAsync(T entity)
        {
            var existing = await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == entity.Id);
            if (existing != null)
            {
                entity.DateCreated = existing.DateCreated;
            }
            entity.DateModified = DateTime.Now;
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
