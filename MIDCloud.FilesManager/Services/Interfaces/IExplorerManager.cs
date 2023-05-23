using MIDCloud.Shared.Models.Interfaces.Explorer;

namespace MIDCloud.FilesManager.Services.Interfaces;

public interface IExplorerManager
{
    IFileManager FileManager { get; }
    
    IDirectoriesManager DirectoryManager { get; }
    
    IElements GetIncludesOfDirectory(string directoryPath);

    List<IFile> GetFilesOfDirectory(string directoryPath);
    
    List<IDirectory> GetDirectoriesOfDirectory(string directoryPath);

    void UploadFiles(List<IFormFile> files, string destinationPath);
}