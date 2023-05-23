using MIDCloud.Shared.Models.Interfaces.Explorer;

namespace MIDCloud.FilesManager.Services.Interfaces;

public interface IDirectoriesManager
{
    IDirectory Get(string dirPath);

    void Create(string dirName, string destination);
}