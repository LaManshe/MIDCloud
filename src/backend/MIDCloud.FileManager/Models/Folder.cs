using MIDCloud.GlobalInterfaces.FileSystem;

namespace MIDCloud.FileManager.Models
{
    internal class Folder : IFolder
    {
        public string Name { get; set; }

        public Folder(string name)
        {
            Name = name;
        }
    }
}
