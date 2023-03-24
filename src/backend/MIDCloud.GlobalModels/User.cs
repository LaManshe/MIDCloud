using MIDCloud.GlobalInterfaces.Users;

namespace MIDCloud.GlobalModels
{
    public class User : IUser
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string RootFolderPath { get; set; }

        public string RootFolderName => Path.GetFileName(Path.GetDirectoryName(RootFolderPath));

        public List<string> PermissionsToFolder { get; set; }
        public string RefreshToken { get; set; }

        public bool IsHavePermission(string folderPath)
        {
            throw new NotImplementedException();
        }
    }
}
