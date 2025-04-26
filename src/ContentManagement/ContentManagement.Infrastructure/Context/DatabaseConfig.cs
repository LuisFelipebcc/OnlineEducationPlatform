using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContentManagement.Infrastructure.Context;

public static class DatabaseConfig
{
    public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
    {
        if (isDevelopment)
        {
            services.AddDbContext<ContentManagementDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("SQLiteConnection")));
        }
        else
        {
            services.AddDbContext<ContentManagementDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}