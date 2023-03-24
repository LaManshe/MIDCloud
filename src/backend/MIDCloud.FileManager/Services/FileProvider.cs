using Ardalis.Result;
using MIDCloud.FileManager.Models;
using MIDCloud.GlobalInterfaces.FileSystem;
using MIDCloud.GlobalInterfaces.Services;

namespace MIDCloud.FileManager.Services
{
    public class FileProvider : IFileProviderService
    {
        public string CreateDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                throw new Exception("Directory already exist");
            }

            try
            {
                var result = Directory.CreateDirectory(path);

                return result.FullName;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ITiles GetTilesOfDirectory(string path)
        {
            List<string> fileEntries = Directory.GetFiles(path).ToList();
            List<string> subdirectoryEntries = Directory.GetDirectories(path).ToList();

            return new TilesOfDirectory(subdirectoryEntries, fileEntries);
        }
    }
}
