using Ardalis.GuardClauses;
using Ardalis.Result;
using JetBrains.Annotations;
using MIDCloud.GlobalInterfaces.FileSystem;
using MIDCloud.GlobalInterfaces.Models;
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

        public string CreateDirectoryFor(IUser user)
        {
            var folderFullPath = Path.Combine(_storagePath, user.Login);

            var result = _fileProvider.CreateDirectory(folderFullPath);

            return result;
        }

        public Result<ITiles> GetTilesOfDirectory([NotNull] IUser user, [NotNull] string folder)
        {
            if (folder == @"/" || folder == @"\")
            {
                folder = string.Empty;
            }

            var folderFullPath = Path.Combine(user.RootFolderPath, folder);

            if (user.IsHavePermission(folderFullPath) is false)
            {
                return Result.Error($"User {user.Login} have not access to this folder");
            }

            var result = _fileProvider.GetTilesOfDirectory(folderFullPath);

            if (result is null)
            {
                return Result.Error("Can not get access to folder");
            }

            return Result.Success(result);
        }
    }
}
