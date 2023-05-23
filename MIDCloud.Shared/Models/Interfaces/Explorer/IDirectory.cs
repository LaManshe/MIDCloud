namespace MIDCloud.Shared.Models.Interfaces.Explorer;

public interface IDirectory
{
    string? Path { get; }
    
    string Name { get; }
}