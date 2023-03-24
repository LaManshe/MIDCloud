using Ardalis.Result;
using MIDCloud.GlobalInterfaces.FileSystem;
using MIDCloud.GlobalInterfaces.Models;
using MIDCloud.GlobalInterfaces.Users;

namespace MIDCloud.GlobalInterfaces.Services
{
    public interface ISystemStorage
    {
        Result<ITiles> GetTilesOfDirectory(IUser user, string folder);
        string CreateDirectoryFor(IUser user);
    }
}
