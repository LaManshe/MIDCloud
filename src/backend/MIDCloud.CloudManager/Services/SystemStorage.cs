using Ardalis.GuardClauses;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using MIDCloud.CloudManager.Models;
using MIDCloud.GlobalInterfaces.FileSystem;
using MIDCloud.GlobalInterfaces.Responses;
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

            ThrowIfDirectoryExist(folderFullPath);

            var result = _fileProvider.CreateDirectory(folderFullPath);

            return result;
        }

        public string CreateDirectoryForNewbee(IUser user, string relFolder)
        {
            var folderFullPath = Path.Combine(_storagePath, relFolder);

            ThrowIfDirectoryExist(folderFullPath);

            var result = _fileProvider.CreateDirectory(folderFullPath);

            return result;
        }

        public string DeleteDirectory(IUser user, string relFolder)
        {
            string folderFullPath = GetCorrectFolderPath(user, relFolder);

            ThrowIfDirectoryNotExist(folderFullPath);
            ThrowIfUserHasNoPermissions(user, folderFullPath);

            _fileProvider.DeleteDirectory(folderFullPath);

            return folderFullPath;
        }

        public ITiles GetTilesOfDirectory([NotNull] IUser user, [NotNull] string relFolder)
        {
            var folderFullPath = GetCorrectFolderPath(user, relFolder);

            ThrowIfDirectoryNotExist(folderFullPath);
            ThrowIfUserHasNoPermissions(user, folderFullPath);

            var result = _fileProvider.GetTilesOfDirectory(folderFullPath);

            if (result is null)
            {
                throw new Exception("Can not get access to folder");
            }

            return result;
        }

        public ITiles GetTilesOfDirectoryLimited(
            IUser user, 
            string relFolder, 
            int limit, 
            int page)
        {
            var folderFullPath = GetCorrectFolderPath(user, relFolder);

            ThrowIfDirectoryNotExist(folderFullPath);
            ThrowIfUserHasNoPermissions(user, folderFullPath);

            var tiles = _fileProvider.GetTilesOfDirectoryLimited(folderFullPath, limit, page);

            if (tiles is null)
            {
                throw new Exception("Can not get access to folder");
            }

            return tiles;
        }

        public void UploadFiles(IUser user, string relFolder, List<IFormFile> files)
        {
            var folderFullPath = GetCorrectFolderPath(user, relFolder);

            ThrowIfDirectoryNotExist(folderFullPath);
            ThrowIfUserHasNoPermissions(user, folderFullPath);

            _fileProvider.UploadFiles(folderFullPath, files);
        }

        public void RemoveFile(IUser user, string filePath)
        {
            var folderFullPath = GetCorrectFilePath(user, filePath);

            ThrowIfDirectoryNotExist(GetFolderOf(folderFullPath));
            ThrowIfUserHasNoPermissions(user, GetFolderOf(folderFullPath));

            _fileProvider.RemoveFile(folderFullPath);
        }

        public IFile GetFile(IUser user, string fileName, string relFolder)
        {
            var folderFullPath = GetCorrectFolderPath(user, relFolder);

            ThrowIfDirectoryNotExist(folderFullPath);
            ThrowIfUserHasNoPermissions(user, folderFullPath);

            var fileNameFull = Path.Combine(folderFullPath, fileName);

            var file = _fileProvider.GetFile(fileNameFull);

            return file;
        }

        public string GetFilePath(IUser user, string fileName, string relFolder)
        {
            var folderFullPath = GetCorrectFolderPath(user, relFolder);

            ThrowIfDirectoryNotExist(folderFullPath);
            ThrowIfUserHasNoPermissions(user, folderFullPath);
            
            return Path.Combine(folderFullPath, fileName);
        }

        public int GetMaxPage(IUser user, string relFolder, int limit, int page)
        {
            var folderFullPath = GetCorrectFolderPath(user, relFolder);

            ThrowIfDirectoryNotExist(folderFullPath);
            ThrowIfUserHasNoPermissions(user, folderFullPath);

            var countTiles = _fileProvider.GetTilesLength(folderFullPath);

            var result = (int)Math.Ceiling((double)countTiles / (double)limit);

            if (result == 1)
            {
                result = 0;
            }

            return result;
        }

        public void RenameFile(IUser user, string fileName, string newName, string relFolder)
        {
            var folderFullPath = GetCorrectFolderPath(user, relFolder);
            
            ThrowIfDirectoryNotExist(folderFullPath);
            ThrowIfUserHasNoPermissions(user, folderFullPath);
            
            var fileNameFull = Path.Combine(folderFullPath, fileName);
            var newFileNameFull = Path.Combine(
                    folderFullPath, $"{newName}{Path.GetExtension(fileNameFull)}");

            _fileProvider.RenameFile(fileNameFull, newFileNameFull);
        }

        public List<string> GetOnlyNameDirectoriesAll(IUser user, string startPath)
        {
            var folderFullPath = GetCorrectFolderPath(user, startPath);
            
            ThrowIfDirectoryNotExist(folderFullPath);
            ThrowIfUserHasNoPermissions(user, folderFullPath);

            return _fileProvider.GetDirectoriesAll(folderFullPath);
        }

        public IBranch<string> GetDirectoriesTree(IUser user, string startPath)
        {
            var folderFullPath = GetCorrectFolderPath(user, startPath);
            
            ThrowIfDirectoryNotExist(folderFullPath);
            ThrowIfUserHasNoPermissions(user, folderFullPath);

            var directories = _fileProvider.GetAll(folderFullPath);

            return new TreeOfStrings(folderFullPath, directories);
        }

        private string GetCorrectFolderPath(IUser user, string folder)
        {
            var paths = folder.Split('\\');

            var folderDestinationPath = Path.Combine(paths);

            var folderFullPath = Path.Combine(user.RootFolderPath, folderDestinationPath);

            return folderFullPath;
        }

        private string GetCorrectFilePath(IUser user, string file)
        {
            var paths = file.Split('\\');

            var folderDestinationPath = Path.Combine(paths);

            var folderFullPath = Path.Combine(user.RootFolderPath, folderDestinationPath);

            return folderFullPath;
        }

        private string GetFolderOf(string path)
        {
            return Path.GetDirectoryName(path) ?? string.Empty;
        }

        private void ThrowIfUserHasNoPermissions(IUser user, string folderFullPath)
        {
            if (user.IsHavePermission(folderFullPath) is false)
            {
                throw new Exception($"User {user.Login} have not access to this folder");
            }
        }

        private void ThrowIfDirectoryNotExist(string folderFullPath)
        {
            if (Directory.Exists(folderFullPath) is false)
            {
                throw new Exception($"Folder {folderFullPath} does not exist");
            }
        }

        private void ThrowIfDirectoryExist(string folderFullPath)
        {
            if (Directory.Exists(folderFullPath))
            {
                throw new Exception($"Folder {folderFullPath} already exist");
            }
        }
    }
}
