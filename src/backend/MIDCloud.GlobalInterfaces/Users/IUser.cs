using MIDCloud.GlobalInterfaces.Base;

namespace MIDCloud.GlobalInterfaces.Users
{
    public interface IUser : IEntity, IMinimalUser
    {
        string Name { get; set; }
        string Login { get; set; }
        string Password { get; set; }
        string RootFolderPath { get; set; }
        string RootFolderName { get; }
        List<string> PermissionsToFolder { get; set; }
        string RefreshToken { get; set; }

        bool IsHavePermission(string folderPath);
    }
}
