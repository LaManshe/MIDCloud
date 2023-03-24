using Ardalis.Result;
using Microsoft.AspNetCore.Http;
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

            var result = Directory.CreateDirectory(path);

            return result.FullName;
        }

        public ITiles GetTilesOfDirectory(string path)
        {
            List<string> fileEntries = Directory.GetFiles(path).ToList();
            List<string> subdirectoryEntries = Directory.GetDirectories(path).ToList();

            return new TilesOfDirectory(subdirectoryEntries, fileEntries);
        }

        public void UploadFiles(string path, List<IFormFile> files)
        {
            foreach (var file in files)
            {
                UploadFile(file, path);
            }
        }

        private void UploadFile(IFormFile file, string path)
        {
            try
            {
                var fileName = Path.GetFileName(file.FileName);

                var filePath = Path.Combine(path, fileName);

                if (System.IO.File.Exists(filePath))
                {
                    throw new Exception($"File {fileName} already exist");
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
