namespace MIDCloud.API.Models.FileModels
{
    public class File
    {
        public string Name => new DirectoryInfo(Path).Name;
        public string Path { get; set; }

        public File(string path)
        {
            Path = path;
        }
    }
}
