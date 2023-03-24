using Microsoft.Extensions.DependencyInjection;
using DAL.Database.Repositories.Base;
using DAL.Database.Repositories;
using MIDCloud.GlobalInterfaces.Models;

namespace DAL
{
    public static class DatabaseRegistrator
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return 
                services.AddTransient<IRepository<User>, UsersRepository>();
        }
    }
}
