using Microsoft.Extensions.DependencyInjection;
using MIDCloud.FileManager.Services;
using MIDCloud.GlobalInterfaces.Services;

namespace MIDCloud.FileManager
{
    public static class FileManagerRegistrator
    {
        public static IServiceCollection AddFileManager(this IServiceCollection services)
        {
            return services
                .AddTransient<IFileProviderService, FileProvider>();
        }
    }
}
