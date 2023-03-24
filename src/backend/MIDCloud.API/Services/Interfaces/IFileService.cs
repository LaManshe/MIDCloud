using Ardalis.Result;
using MIDCloud.API.Models;
using MIDCloud.API.Models.FileModels;
using MIDCloud.API.Models.ResponseModels;

namespace MIDCloud.API.Services.Interfaces
{
    public interface IFileService
    {
        Result<TilesOfFolder> GetTiles(string folderPath);
        Result<TilesOfFolder> UploadFiles(List<IFormFile> files, string storagePath);
        Result<DirectoryInfo> CreateFolder(string folderPath);
        Result DeleteFolder(string folderPath);
    }
}
