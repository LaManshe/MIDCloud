using MIDCloud.Shared.Models.Interfaces.Explorer;

namespace MIDCloud.FilesManager.Services.Interfaces;

public interface IFileManager
{
    IFile Get(string filePath);

    void Upload(IFormFile file, string destination);

    void Rename(string filePath, string newName);

    void Delete(string filePath);

    void Move(string filePath, string destination);
}