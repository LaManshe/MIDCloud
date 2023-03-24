using Ardalis.GuardClauses;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using MIDCloud.CloudManager.Models;
using MIDCloud.GlobalInterfaces.FileSystem;
using MIDCloud.GlobalInterfaces.Models;
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
                registeredUser = registeredUser is null ? _userService.Get(user) : registeredUser;

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
    }
}
