using Microsoft.Extensions.DependencyInjection;
using MIDCloud.CloudManager.Services;
using MIDCloud.FileManager;
using MIDCloud.GlobalInterfaces.Services;

namespace MIDCloud.CloudManager
{
    public static class ServicesRegistrator
    {
        public static IServiceCollection RegisterAllServices(this IServiceCollection services)
        {
            return services
                .AddCloudServices()
                .AddFileManager()
                ;
        }
    }
}
