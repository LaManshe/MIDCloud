using Ardalis.Result;
using MIDCloud.API.Models.UserModels;
using MIDCloud.GlobalInterfaces.Users;

namespace MIDCloud.API.Services.Interfaces
{
    public interface IUserService
    {
        IUser GetUserById(int id);
        IUser GetUserByRefreshToken(string refreshToken);
        AuthResponse Authenticate(string login, string password);
        Result Register(string name, string login, string password, string rootFolderPath);
        Result SetPermissionsForFolder(IUser user, string folderPath);
        Result SetPermissionsForFolder(int userId, string folderPath);
        bool IsHavePermissionToFolder(IUser user, string folderPath);
    }
}
