using Microsoft.EntityFrameworkCore;
using StudentManagement.Persistence.Model;

namespace StudentManagement.Persistence.DataBaseContext
{
    public class MenuDbContext : DbContext
    {
        public MenuDbContext(DbContextOptions<MenuDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>()
                .HasKey(m => new { m.MenuId, m.MenuTypeId });
        }

        public DbSet<IngredientCombo> IngredientCombos { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuName> MenuNames { get; set; }
        public DbSet<MenuType> MenuTypes { get; set; }
    }
}
