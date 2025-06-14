using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyDiary.Infrastructure.Persistence;

namespace MyDiary.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        try
        {
            var connectionString = configuration.GetConnectionString("DiaryDb");

            services.AddDbContext<MyDiaryDbContext>(options =>
                options.UseNpgsql(connectionString)
                    .EnableSensitiveDataLogging()
            );
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}