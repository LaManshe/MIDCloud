using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using MIDCloud.FileManager.Models;
using MIDCloud.GlobalInterfaces.FileSystem;
using MIDCloud.GlobalInterfaces.Requests;
using MIDCloud.GlobalInterfaces.Services;
using File = MIDCloud.FileManager.Models.File;

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

        public void DeleteDirectory(string path)
        {
            if (Directory.Exists(path) is false)
            {
                throw new Exception("Directory doesn't exist");
            }

            Directory.Delete(path, true);
        }

        public IFile GetFile(string path)
        {
            if (System.IO.File.Exists(path) is false)
            {
                throw new Exception($"File doesn't exist");
            }

            return new File(path);
        }

        public void RenameFile(string oldName, string newName)
        {
            if (System.IO.File.Exists(oldName) is false)
            {
                throw new Exception($"File doesn't exist");
            }
            
            System.IO.File.Move(oldName, newName);
        }

        public ITiles GetTilesOfDirectory(string path)
        {
            List<string> subdirectoryEntries = Directory.GetDirectories(path).ToList();
            List<string> fileEntries = Directory.GetFiles(path).ToList();

            return new TilesOfDirectory(subdirectoryEntries, fileEntries);
        }

        public ITiles GetTilesOfDirectoryLimited(
            string path, 
            int limit, 
            int page)
        {
            List<string> subdirectoryEntries = Directory.GetDirectories(path).ToList();
            List<string> fileEntries = Directory.GetFiles(path).ToList();

            int startLimit = page * limit;
            int endLimit = startLimit + limit;

            List<string> stringsElements = new List<string>();

            stringsElements.AddRange(subdirectoryEntries);
            stringsElements.AddRange(fileEntries);

            stringsElements = stringsElements.Skip(startLimit).Take(endLimit - startLimit).ToList();

            subdirectoryEntries = subdirectoryEntries.Intersect(stringsElements).ToList();
            fileEntries = fileEntries.Intersect(stringsElements).ToList();

            return new TilesOfDirectory(subdirectoryEntries, fileEntries);
        }

        public void UploadFiles(string path, List<IFormFile> files)
        {
            foreach (var file in files)
            {
                UploadFile(file, path);
            }
        }

        public void RemoveFile(string filePath)
        {
            System.IO.File.Delete(filePath);
        }

        public int GetTilesLength(string path)
        {
            var subdirectoryEntries = Directory.GetDirectories(path).ToList();
            var fileEntries = Directory.GetFiles(path).ToList();

            return subdirectoryEntries.Count + fileEntries.Count;
        }

        public List<string> GetDirectoriesAll(string startPath)
        {
            return Directory
                .GetDirectories(startPath, "*", SearchOption.AllDirectories)
                .ToList();
        }

        public List<string> GetAll(string path)
        {
            throw new NotImplementedException();
        }

        private void UploadFile(IFormFile file, string path)
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
    }
}
