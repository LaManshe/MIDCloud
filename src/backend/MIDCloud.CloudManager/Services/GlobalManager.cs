using Ardalis.GuardClauses;
using Ardalis.Result;
using MIDCloud.CloudManager.Models;
using MIDCloud.GlobalInterfaces.Models;
using MIDCloud.GlobalInterfaces.Responses;
using MIDCloud.GlobalInterfaces.Services;
using MIDCloud.GlobalInterfaces.Users;

namespace MIDCloud.CloudManager.Services
{
    public class GlobalManager : ICloudManager
    {
        public ISystemStorage SystemStorage { get; set; }

        public IUserService UserService { get; set; }

        public IAuthenticateService AuthenticateService => throw new NotImplementedException();

        public GlobalManager(
            ISystemStorage systemStorage, 
            IUserService userService)
        {
            SystemStorage = Guard.Against.Null(systemStorage, nameof(systemStorage));
            UserService = Guard.Against.Null(userService, nameof(userService));
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

                var registeredUser = UserService.AddToDatabase(user);

                var createdDirectory = SystemStorage.CreateDirectoryFor(registeredUser);

                var givePermission = UserService.AddPermissionsAsRoot(registeredUser, createdDirectory);

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
                registeredUser = registeredUser is null ? UserService.Get(user) : registeredUser;

                (string, string) tokens = UserService.GenerateTokens(registeredUser);

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
                var registeredUser = UserService.Get(refreshToken);

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
    }
}
