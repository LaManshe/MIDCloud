using Ardalis.GuardClauses;
using MIDCloud.FilesManager.Services.Interfaces;
using MIDCloud.Shared.Models.Interfaces.Explorer;
using File = MIDCloud.FilesManager.Models.File;

namespace MIDCloud.FilesManager.Services;

public class FileManager : IFileManager
{
    public IFile Get(string filePath)
    {
        Guard.Against.NullOrEmpty(filePath, nameof(filePath));
        
        return new File(filePath);
    }

    public void Upload(IFormFile file, string destination)
    {
        var fileName = Path.GetFileName(file.FileName);

        var filePath = Path.Combine(destination, fileName);

        ThrowIfFileExist(filePath);
        ThrowIfDirectoryDoesntExist(destination);

        using var stream = new FileStream(filePath, FileMode.Create);
    
        file.CopyTo(stream);
    }

    public void Rename(string filePath, string newName)
    {
        var directory = Path.GetDirectoryName(filePath);

        ThrowIfDirectoryDoesntExist(directory ?? string.Empty);
        
        var newPath = Path.Combine(directory!, newName);
        
        System.IO.File.Move(filePath, newPath);
    }

    public void Delete(string filePath)
    {
        ThrowIfFileDoesntExist(filePath);
        
        System.IO.File.Delete(filePath);
    }

    public void Move(string filePath, string destination)
    {
        ThrowIfFileDoesntExist(filePath);
        ThrowIfDirectoryDoesntExist(destination);

        var destinationFilePath = Path.Combine(
            destination, 
            Path.GetFileName(filePath));
        
        System.IO.File.Move(filePath, destinationFilePath);
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
    
    private static void ThrowIfFileDoesntExist(string filePath)
    {
        if (System.IO.File.Exists(filePath) is false)
        {
            throw new Exception($"File {filePath} doesn't exist");
        }
    }
}