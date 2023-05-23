using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MIDCloud.GlobalInterfaces.FileSystem;
using MIDCloud.GlobalInterfaces.Requests;
using MIDCloud.GlobalInterfaces.Responses;
using MIDCloud.GlobalInterfaces.Users;

namespace MIDCloud.GlobalInterfaces.Services
{
    public interface ICloudManager
    {
        Result RegisterUser(IMinimalUser userData);
        Result<IAuthenticate> AuthenticateUser(IMinimalUser user, IUser registeredUser = null);
        Result<IAuthenticate> RefreshAuthenticate(string refreshToken);

        Result RegisterFolder(IUser user, string folderName);
        Result UnregisterFolders(IUser user, string[] foldersName);
        Result<ITiles> GetTilesOfDirectory(IUser user, string folderName);
        Result<ITiles> GetTilesOfDirectoryLimited(IUser user, string folderName, string limit, string page);
        Result UploadFiles(IUser user, string folderName, List<IFormFile> files);
        Result RemoveFiles(IUser user, string[] filesPath);
        
        Result<IFile> GetFile(IUser user, string fileName, string folderName);
        Result<string> GetVideoFilePath(IUser user, string fileName, string folderName);
        Result RenameFile(IUser user, string fileName, string newName, string path);

        Result<IBranch<string>> GetFoldersTree(IUser user, string startPath);
        Result<List<string>> GetOnlyNameFoldersAll(IUser user, string startPath);

        Result<int> GetMaxPage(IUser user, string folderName, string limit, string page);
    }
}
