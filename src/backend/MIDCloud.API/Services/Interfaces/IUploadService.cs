using Ardalis.Result;
using MIDCloud.API.Models;

namespace MIDCloud.API.Services.Interfaces
{
    public interface IUploadService
    {
        UploadStatus UploadFiles(List<IFormFile> files, string storagePath);

        Result UploadFile(IFormFile file, string storagePath);
    }
}
