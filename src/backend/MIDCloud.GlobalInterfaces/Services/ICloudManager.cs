using Ardalis.Result;
using MIDCloud.GlobalInterfaces.Responses;
using MIDCloud.GlobalInterfaces.Users;

namespace MIDCloud.GlobalInterfaces.Services
{
    public interface ICloudManager
    {
        ISystemStorage SystemStorage { get; }
        IUserService UserService { get; }
        IAuthenticateService AuthenticateService { get; }

        Result RegisterUser(IMinimalUser userData);
        Result<IAuthenticate> AuthenticateUser(IMinimalUser user, IUser registeredUser = null);
        Result<IAuthenticate> RefreshAuthenticate(string refreshToken);
    }
}
