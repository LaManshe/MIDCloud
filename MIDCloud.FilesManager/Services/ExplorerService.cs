using MIDCloud.FilesManager.Models;
using MIDCloud.FilesManager.Services.Interfaces;
using MIDCloud.Shared.Models.Interfaces.Explorer;

namespace MIDCloud.FilesManager.Services;

public class ExplorerService : IExplorerManager
{
    public IFileManager FileManager { get; }
    
    public IDirectoriesManager DirectoryManager { get; }

    public ExplorerService()
    {
        FileManager = new FileManager();
        DirectoryManager = new DirectoriesManager();
    }

    public IElements GetIncludesOfDirectory(string directoryPath)
    {
        return new FilesAndDirectories(
            GetFilesOfDirectory(directoryPath),
            GetDirectoriesOfDirectory(directoryPath));
    }

    public List<IFile> GetFilesOfDirectory(string directoryPath)
    {
        var resultFiles = new List<IFile>();
        
        var fileEntries = 
            System.IO.Directory.GetFiles(directoryPath).ToList();
        
        fileEntries.ForEach(file => 
            resultFiles.Add(FileManager.Get(file)));

        return resultFiles;
    }

    public List<IDirectory> GetDirectoriesOfDirectory(string directoryPath)
    {
        var resultDirectories = new List<IDirectory>();
        
        var subdirectoryEntries = 
            System.IO.Directory
                .GetDirectories(directoryPath)
                .ToList();
        
        subdirectoryEntries.ForEach(dir => 
            resultDirectories.Add(DirectoryManager.Get(dir)));

        return resultDirectories;
    }

    public void UploadFiles(List<IFormFile> files, string destinationPath)
    {
        files.ForEach(file => UploadFile(file, destinationPath));
    }

    private static void UploadFile(IFormFile file, string destinationPath)
    {
        var fileName = Path.GetFileName(file.FileName);

        var filePath = Path.Combine(destinationPath, fileName);

        ThrowIfFileExist(filePath);
        ThrowIfDirectoryDoesntExist(destinationPath);

        using var stream = new FileStream(filePath, FileMode.Create);
        
        file.CopyTo(stream);
    }

    private static void ThrowIfDirectoryDoesntExist(string directoryPath)
    {
        if (System.IO.Directory.Exists(directoryPath) is false)
        {
            throw new Exception($"Directory {directoryPath} doesn't exist");
        }
    }

    private static void ThrowIfFileExist(string filePath)
    {
        if (System.IO.File.Exists(filePath))
        {
            throw new Exception($"File {filePath} already exist");
        }
    }
}