using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using MIDCloud.GlobalInterfaces.FileSystem;
using MIDCloud.GlobalInterfaces.Responses;
using MIDCloud.GlobalInterfaces.Users;

namespace MIDCloud.GlobalInterfaces.Services
{
    public interface ICloudManager
    {
        Result RegisterUser(IMinimalUser userData);
        Result<IAuthenticate> AuthenticateUser(IMinimalUser user, IUser registeredUser = null);
        Result<IAuthenticate> RefreshAuthenticate(string refreshToken);

        Result RegisterFolder(IUser user, string folderName);
        Result<ITiles> GetTilesOfDirectory(IUser user, string folderName);
        Result UploadFiles(IUser user, string folderName, List<IFormFile> files);
    }
}
