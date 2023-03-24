using MIDCloud.API.Models.ResponseModels.Interface;

namespace MIDCloud.API.Models.ResponseModels
{
    public class TilesOfFolder
    {
        public List<IFolder> Folders { get; set; }
        public List<IFile> Files { get; set; }

        public TilesOfFolder(List<string> folders, List<string> files)
        {
            Folders = new List<IFolder>();
            Files = new List<IFile>();

            foreach (var folder in folders)
            {
                Folders.Add(new FolderTile(folder));
            }
            foreach (var file in files)
            {
                Files.Add(new FileTile(file));
            }
        }
    }
}
