using Microsoft.EntityFrameworkCore;
using StudentManagement.Persistence.DataBaseContext;
using StudentManagement.Persistence.Repository;

namespace StudentManagement.Persistence
{
    public static class PersistenceRegistration
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StudentDbContext>(options =>
            {
                options.UseMySql(
                    configuration.GetConnectionString("DefaultConnectionString"),
                    new MySqlServerVersion(new Version(11, 4, 5))
                );
            });

            services.AddScoped<IStudentRepository, StudentRepository>();

            // 필요시 GenericRepository 등 추가 등록
            // services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }
    }
}
