using Ardalis.GuardClauses;
using MIDCloud.Shared.Models.Interfaces.Explorer;

namespace MIDCloud.FilesManager.Models;

public class Directory : IDirectory
{
    public string Path { get; }
    
    public string Name { get; }

    public Directory(string path)
    {
        Path = Guard.Against.NullOrEmpty(path, nameof(path));

        Name = GetFolderName() ?? "Unknown";
    }

    private string? GetFolderName()
    {
        return System.IO.Path
            .GetDirectoryName(Path)!
            .Split('\\')
            .Last();
    }
}