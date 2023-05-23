using MIDCloud.FilesManager.Models;
using MIDCloud.FilesManager.Services.Interfaces;
using MIDCloud.Shared.Models.Interfaces.Explorer;
using Directory = MIDCloud.FilesManager.Models.Directory;

namespace MIDCloud.FilesManager.Services;

public class DirectoriesManager : IDirectoriesManager
{
    public IDirectory Get(string dirPath)
    {
        return new Directory(dirPath);
    }

    public void Create(string dirName, string destination)
    {
        var fullPath = Path.Combine(dirName, destination);
        
        ThrowIfDirectoryExist(fullPath);
        
        System.IO.Directory.CreateDirectory(fullPath);
    }
    
    private void ThrowIfDirectoryExist(string directoryPath)
    {
        if (System.IO.Directory.Exists(directoryPath) is false)
        {
            throw new Exception($"Directory {directoryPath} already exist");
        }
    }
}