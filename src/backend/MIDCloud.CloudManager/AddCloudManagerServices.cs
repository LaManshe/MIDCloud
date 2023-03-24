using Microsoft.Extensions.DependencyInjection;
using MIDCloud.CloudManager.Services;
using MIDCloud.GlobalInterfaces.Services;

namespace MIDCloud.CloudManager
{
    public static class AddCloudManagerServices
    {
        public static IServiceCollection AddCloudServices(this IServiceCollection services)
        {
            return services
                .AddTransient<ICloudManager, GlobalManager>()
                .AddTransient<IUserService, UserService>()
                .AddTransient<ISystemStorage, SystemStorage>()
                ;
        }
    }
}
