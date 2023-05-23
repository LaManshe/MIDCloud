using MIDCloud.GlobalInterfaces.FileSystem;

namespace MIDCloud.GlobalInterfaces.Responses;

public interface IBranch<T>
{
    string Name { get; set; }
    
    List<IBranch<T>> Branches { get; set; }
}