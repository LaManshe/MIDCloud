namespace MIDCloud.API.Models.FileModels
{
    public class UploadFilesModel
    {
        public List<IFormFile> Files { get; set; }
        public string Folder { get; set; }
    }
}
