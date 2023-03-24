using Microsoft.AspNetCore.Http;

namespace MIDCloud.GlobalInterfaces.Requests
{
    public interface IUploadFilesRequest
    {
        List<IFormFile> Files { get; set; }
        string Folder { get; set; }
    }
}
