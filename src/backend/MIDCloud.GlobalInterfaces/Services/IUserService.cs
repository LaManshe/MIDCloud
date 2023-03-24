using MIDCloud.GlobalInterfaces.Models;
using MIDCloud.GlobalInterfaces.Users;

namespace MIDCloud.GlobalInterfaces.Services
{
    public interface IUserService
    {
        User Get(int id);
        User Get(IMinimalUser user);
        User Get(string refreshToken);
        User AddToDatabase(IUser user);
        bool AddPermissions(IUser user, string folder);
        bool AddPermissionsAsRoot(IUser user, string folder);
        (string, string) GenerateTokens(IUser user);
    }
}
