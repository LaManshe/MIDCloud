using MIDCloud.GlobalInterfaces.Requests;

namespace MIDCloud.API.Models.FileModels
{
    public class UploadFilesModel : IUploadFilesRequest
    {
        public List<IFormFile> Files { get; set; }
        public string Folder { get; set; }
    }
}
