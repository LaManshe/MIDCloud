using MIDCloud.GlobalInterfaces.FileSystem;

namespace MIDCloud.FileManager.Models
{
    internal class Folder : IFolder
    {
        private readonly string _path;

        public string Name { get; set; }

        public Folder(string path)
        {
            _path = path;

            Name = Path.GetFileName(_path);
        }
    }
}
