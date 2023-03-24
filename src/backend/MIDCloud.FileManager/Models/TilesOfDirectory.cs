using MIDCloud.GlobalInterfaces.FileSystem;

namespace MIDCloud.FileManager.Models
{
    internal class TilesOfDirectory : ITiles
    {
        public List<IFolder> Folders { get; set; }
        public List<IFile> Files { get; set; }

        public TilesOfDirectory(List<string> folders, List<string> files)
        {
            Folders = new List<IFolder>();
            Files = new List<IFile>();

            foreach (var folder in folders)
            {
                Folders.Add(new Folder(folder));
            }
            foreach (var file in files)
            {
                Files.Add(new File(file));
            }
        }
    }
}
