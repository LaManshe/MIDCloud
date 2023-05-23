using Ardalis.GuardClauses;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using MIDCloud.CloudManager.Models;
using MIDCloud.FileManager.Models;
using MIDCloud.GlobalInterfaces.FileSystem;
using MIDCloud.GlobalInterfaces.Models;
using MIDCloud.GlobalInterfaces.Requests;
using MIDCloud.GlobalInterfaces.Responses;
using MIDCloud.GlobalInterfaces.Services;
using MIDCloud.GlobalInterfaces.Users;

namespace MIDCloud.CloudManager.Services
{
    public class GlobalManager : ICloudManager
    {
        private readonly ISystemStorage _systemStorage;
        private readonly IUserService _userService;

        public GlobalManager(
            ISystemStorage systemStorage, 
            IUserService userService)
        {
            _systemStorage = Guard.Against.Null(systemStorage, nameof(systemStorage));
            _userService = Guard.Against.Null(userService, nameof(userService));
        }

        public Result RegisterUser(IMinimalUser userData)
        {
            try
            {
                var user = new User()
                {
                    Name = "TODO",
                    Login = userData.Login,
                    Password = userData.Password,
                    PermissionsToFolder = new List<string>(),
                    RefreshToken = string.Empty,
                    RootFolderPath = string.Empty
                };

                var registeredUser = _userService.AddToDatabase(user);

                var createdDirectory = _systemStorage.CreateDirectoryForNewbee(registeredUser, user.Login);

                var givePermission = _userService.AddPermissionsAsRoot(registeredUser, createdDirectory);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public Result<IAuthenticate> AuthenticateUser(IMinimalUser user, IUser? registeredUser = null)
        {
            try
            {
                registeredUser = registeredUser ?? _userService.Get(user);

                (string, string) tokens = _userService.GenerateTokens(registeredUser);

                var result = new AuthenticateResponse(tokens);

                return Result.Success((IAuthenticate)result);
            }
            catch (InvalidOperationException)
            {
                return Result.Error($"User {user.Login} doesn't registered");
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public Result<IAuthenticate> RefreshAuthenticate(string refreshToken)
        {
            try
            {
                var registeredUser = _userService.Get(refreshToken);

                return AuthenticateUser(registeredUser, registeredUser);
            }
            catch (InvalidOperationException)
            {
                return Result.Error($"User doesn't registered or user don't have cookie refresh token");
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public Result RegisterFolder(IUser user, string folderName)
        {
            try
            {
                var folderPath = _systemStorage.CreateDirectory(user, folderName);

                if (_userService.AddPermissions(user, folderPath))
                {
                    return Result.Success();
                }

                throw new Exception($"Can't give permissions for user {user.Login}");
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public Result UnregisterFolders(IUser user, string[] foldersName)
        {
            try
            {
                foreach (var folder in foldersName)
                {
                    var deletedFolder = _systemStorage.DeleteDirectory(user, folder);

                    var deleted = _userService.RemovePermissions(user, folder);

                    if (deleted is false)
                    {
                        throw new Exception($"Can't delete permissions user {user.Login}");
                    }
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public Result<ITiles> GetTilesOfDirectory(IUser user, string folderName)
        {
            try
            {
                var result = _systemStorage.GetTilesOfDirectory(user, folderName);

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public Result<ITiles> GetTilesOfDirectoryLimited(
            IUser user, 
            string folderName, 
            string limit, 
            string page)
        {
            try
            {
                int limitInt = int.Parse(limit);
                int pageInt = int.Parse(page);

                var result = _systemStorage.GetTilesOfDirectoryLimited(user, folderName, limitInt, pageInt);

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public Result UploadFiles(IUser user, string folderName, List<IFormFile> files)
        {
            try
            {
                _systemStorage.UploadFiles(user, folderName, files);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public Result RemoveFiles(IUser user, string[] filesPath)
        {
            try
            {
                foreach (var file in filesPath)
                {
                    _systemStorage.RemoveFile(user, file);
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public Result<IFile> GetFile(IUser user, string fileName, string folderName)
        {
            try
            {
                var file = _systemStorage.GetFile(user, fileName, folderName);

                return Result.Success(file);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public Result<string> GetVideoFilePath(IUser user, string fileName, string folderName)
        {
            try
            {
                var fileResult = GetFile(user, fileName, folderName);

                if (fileResult.Value.Extension is not VideoExtension)
                {
                    throw new Exception($"File ${fileResult.Value.Name} is not a video file.");
                }

                var resultPath = _systemStorage.GetFilePath(user, fileName, folderName);

                return Result.Success(resultPath);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public Result RenameFile(IUser user, string fileName, string newName, string path)
        {
            try
            {
                _systemStorage.RenameFile(user, fileName, newName, path);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public Result<IBranch<string>> GetFoldersTree(IUser user, string startPath)
        {
            try
            {
                var tilesFoldersResult = _systemStorage.GetDirectoriesTree(user, startPath);

                return Result.Success(tilesFoldersResult);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public Result<List<string>> GetOnlyNameFoldersAll(IUser user, string startPath)
        {
            try
            {
                var result = _systemStorage.GetOnlyNameDirectoriesAll(user, startPath);

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public Result<int> GetMaxPage(IUser user, string folderName, string limit, string page)
        {
            try
            {
                int limitInt = int.Parse(limit);
                int pageInt = int.Parse(page);

                var result = _systemStorage.GetMaxPage(user, folderName, limitInt, pageInt);

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }
    }
}
