using Microsoft.EntityFrameworkCore;
using StudentManagement.Persistence.DataBaseContext;
using StudentManagement.Persistence.Repository;

namespace StudentManagement.Persistence
{
    public static class PersistenceRegistration
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MenuDbContext>(options =>
            {
                options.UseMySql(
                    configuration.GetConnectionString("DefaultConnectionString"),
                    new MySqlServerVersion(new Version(11, 4, 5))
                );
            });
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IMenuNameRepository, MenuNameRepository>();
            services.AddScoped<IMenuTypeRepository, MenuTypeRepository>();
            services.AddScoped<IIngredientComboRepository, IngredientComboRepository>();

            return services;
        }
    }
}
