using MIDCloud.FilesManager.Services.Interfaces;
using MIDCloud.Shared.Models.Interfaces.Explorer;

namespace MIDCloud.FilesManager.Models;

public class FilesAndDirectories : IElements
{
    public List<IFile> Files { get; }
    
    public List<IDirectory> Directories { get; }

    public FilesAndDirectories(List<IFile> files, List<IDirectory> directories)
    {
        Files = files;
        Directories = directories;
    }
}