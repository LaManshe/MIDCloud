using MIDCloud.GlobalInterfaces.FileSystem.FileExtensions.Base;

namespace MIDCloud.GlobalInterfaces.FileSystem
{
    public interface IFile
    {
        string Name { get; set; }
        DateTime UploadTime { get; set; }

        object Extension { get; }
    }
}
