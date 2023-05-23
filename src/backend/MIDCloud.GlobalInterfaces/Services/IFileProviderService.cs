using Microsoft.AspNetCore.Http;
using MIDCloud.GlobalInterfaces.FileSystem;
using MIDCloud.GlobalInterfaces.Requests;

namespace MIDCloud.GlobalInterfaces.Services
{
    public interface IFileProviderService
    {
        ITiles GetTilesOfDirectory(string path);
        ITiles GetTilesOfDirectoryLimited(string path, int limit, int page);
        string CreateDirectory(string path);
        void DeleteDirectory(string path);
        void UploadFiles(string path, List<IFormFile> files);
        void RemoveFile(string filePath);
        IFile GetFile(string path);
        void RenameFile(string oldName, string newName);
        int GetTilesLength(string path);
        List<string> GetDirectoriesAll(string startPath);
        List<string> GetAll(string path);
    }
}
