using MIDCloud.Shared.Models.Interfaces.Explorer;

namespace MIDCloud.FilesManager.Services.Interfaces;

public interface IElements
{
    List<IFile> Files { get; }
    
    List<IDirectory> Directories { get; }
}