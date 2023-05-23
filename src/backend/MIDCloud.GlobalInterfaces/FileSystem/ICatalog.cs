namespace MIDCloud.GlobalInterfaces.FileSystem;

public interface ICatalog<T>
{
    T Name { get; set; }
    
    List<T> Catalogs { get; set; }
    
    List<T> Files { get; set; }
}