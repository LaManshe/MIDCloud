using Microsoft.AspNetCore.Http;
using MIDCloud.GlobalInterfaces.FileSystem;

namespace MIDCloud.GlobalInterfaces.Services
{
    public interface IFileProviderService
    {
        ITiles GetTilesOfDirectory(string path);
        string CreateDirectory(string path);
        void UploadFiles(string path, List<IFormFile> files);
    }
}
