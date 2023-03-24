using MIDCloud.GlobalInterfaces.FileSystem;

namespace MIDCloud.FileManager.Models
{
    internal class File : IFile
    {
        public string Name { get; set; }

        public File(string name)
        {
            Name = name;
        }
    }
}
