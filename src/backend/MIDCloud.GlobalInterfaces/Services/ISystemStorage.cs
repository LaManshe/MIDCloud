using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using MIDCloud.GlobalInterfaces.FileSystem;
using MIDCloud.GlobalInterfaces.Models;
using MIDCloud.GlobalInterfaces.Requests;
using MIDCloud.GlobalInterfaces.Responses;
using MIDCloud.GlobalInterfaces.Users;

namespace MIDCloud.GlobalInterfaces.Services
{
    public interface ISystemStorage
    {
        ITiles GetTilesOfDirectory(IUser user, string relFolder);
        ITiles GetTilesOfDirectoryLimited(IUser user, string relFolder, int limit, int page);
        string CreateDirectory(IUser user, string relFolder);
        string CreateDirectoryForNewbee(IUser user, string relFolder);
        string DeleteDirectory(IUser user, string relFolder);
        void UploadFiles(IUser user, string relFolder, List<IFormFile> files);
        void RemoveFile(IUser user, string filePath);
        IFile GetFile(IUser user, string fileName, string relFolder);
        string GetFilePath(IUser user, string fileName, string relFolder);
        int GetMaxPage(IUser user, string relFolder, int limit, int page);
        void RenameFile(IUser user, string fileName, string newName, string relFolder);
        List<string> GetOnlyNameDirectoriesAll(IUser user, string startPath);
        IBranch<string> GetDirectoriesTree(IUser user, string startPath);
    }
}
