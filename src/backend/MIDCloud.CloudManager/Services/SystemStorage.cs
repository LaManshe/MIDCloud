using Ardalis.GuardClauses;
using Ardalis.Result;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using MIDCloud.GlobalInterfaces.FileSystem;
using MIDCloud.GlobalInterfaces.Services;
using MIDCloud.GlobalInterfaces.Users;

namespace MIDCloud.CloudManager.Services
{
    public class SystemStorage : ISystemStorage
    {
        private readonly string _storagePath;

        private readonly IFileProviderService _fileProvider;

        public SystemStorage(IFileProviderService fileProvider)
        {
            _fileProvider = Guard.Against.Null(fileProvider, nameof(fileProvider));

            var _appPath = Environment.CurrentDirectory;
            var _storageRelPath = Path.Combine("data");

            _storagePath = Path.Combine(_appPath, _storageRelPath);
        }

        public string CreateDirectory(IUser user, string relFolder)
        {
            string folderFullPath = GetCorrectFolderPath(user, relFolder);

            var result = _fileProvider.CreateDirectory(folderFullPath);

            return result;
        }

        public string CreateDirectoryForNewbee(IUser user, string relFolder)
        {
            var folderFullPath = Path.Combine(_storagePath, relFolder);

            var result = _fileProvider.CreateDirectory(folderFullPath);

            return result;
        }

        public ITiles GetTilesOfDirectory([NotNull] IUser user, [NotNull] string relFolder)
        {
            var folderFullPath = GetCorrectFolderPath(user, relFolder);

            if (user.IsHavePermission(folderFullPath) is false)
            {
                throw new Exception($"User {user.Login} have not access to this folder");
            }

            var result = _fileProvider.GetTilesOfDirectory(folderFullPath);

            if (result is null)
            {
                throw new Exception("Can not get access to folder");
            }

            return result;
        }

        public void UploadFiles(IUser user, string relFolder, List<IFormFile> files)
        {
            var folderFullPath = GetCorrectFolderPath(user, relFolder);

            if (Directory.Exists(folderFullPath) is false)
            {
                throw new Exception($"Fodler {folderFullPath} does not exist");
            }

            if (user.IsHavePermission(folderFullPath) is false)
            {
                throw new Exception($"User {user.Login} have not access to this folder");
            }

            _fileProvider.UploadFiles(folderFullPath, files);

            return;
        }

        private static string GetCorrectFolderPath(IUser user, string folder)
        {
            var paths = folder.Split('\\');

            var folderDestinationPath = Path.Combine(paths);

            var folderFullPath = Path.Combine(user.RootFolderPath, folderDestinationPath);

            return folderFullPath;
        }
    }
}
