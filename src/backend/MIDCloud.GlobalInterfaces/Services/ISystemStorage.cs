using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using MIDCloud.GlobalInterfaces.FileSystem;
using MIDCloud.GlobalInterfaces.Models;
using MIDCloud.GlobalInterfaces.Users;

namespace MIDCloud.GlobalInterfaces.Services
{
    public interface ISystemStorage
    {
        ITiles GetTilesOfDirectory(IUser user, string relFolder);
        string CreateDirectory(IUser user, string relFolder);
        string CreateDirectoryForNewbee(IUser user, string relFolder);
        void UploadFiles(IUser user, string relFolder, List<IFormFile> files);
    }
}
