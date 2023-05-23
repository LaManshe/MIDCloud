using MIDCloud.Shared.Models.Interfaces.Explorer;

namespace MIDCloud.APIGateway.Models;

public class File : IFile
{
    public string? Path { get; }
    
    public string Name { get; }
}