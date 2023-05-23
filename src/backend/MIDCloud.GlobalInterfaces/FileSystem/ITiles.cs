using MIDCloud.GlobalInterfaces.Requests;

namespace MIDCloud.GlobalInterfaces.FileSystem
{
    public interface ITiles
    {
        List<IFolder> Folders { get; set; }
        List<IFile> Files { get; set; }

        void SortFiles(SortFileTypeEnum sortType);
    }
}
