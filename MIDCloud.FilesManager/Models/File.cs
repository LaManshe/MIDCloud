using Ardalis.GuardClauses;
using MIDCloud.Shared.Models.Interfaces.Explorer;

namespace MIDCloud.FilesManager.Models;

public class File : IFile
{
    public string Path { get; }
    
    public string Name { get; }

    public File(string path)
    {
        Path = Guard.Against.NullOrEmpty(path, nameof(path));

        Name = GetFileName() ?? "Unknown";
    }

    private string? GetFileName()
    {
        return System.IO.Path.GetFileName(Path);
    }
}