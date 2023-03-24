using DAL;
using Microsoft.EntityFrameworkCore;
using MIDCloud.CloudManager;

namespace MIDCloud.API.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .RegisterAllServices();
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddDbContext<AppDbContext>(
                    options => options.UseSqlServer(configuration.GetConnectionString("Local_MSSQL")))
                .AddRepositories()
                ;
        }
    }
}
