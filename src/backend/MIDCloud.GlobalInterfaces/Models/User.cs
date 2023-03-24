using MIDCloud.GlobalInterfaces.Users;

namespace MIDCloud.GlobalInterfaces.Models
{
    public class User : Entity, IUser
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string RootFolderPath { get; set; }
        public List<string> PermissionsToFolder { get; set; }
        public string RefreshToken { get; set; }
        public string RootFolderName => Path.GetFileName(Path.GetDirectoryName(RootFolderPath));

        public bool IsHavePermission(string folderPath)
        {
            return PermissionsToFolder.Contains(folderPath);
        }
    }
}
