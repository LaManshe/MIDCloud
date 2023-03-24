using File = MIDCloud.API.Models.FileModels.File;

namespace MIDCloud.API.Models.FileModels
{
    public class Folder
    {
        public List<Folder> Folders { get; set; }
        public List<File> Files { get; set; }

        public string Name => new DirectoryInfo(Path).Name;
        public string Path { get; set; }

        public Folder()
        {
            Folders = new List<Folder>();
            Files = new List<File>();
        }

        public Folder(string path) : this()
        {
            Path = path;
        }
    }
}
 