using Ardalis.Result;
using MIDCloud.API.Models;
using MIDCloud.API.Models.FileModels;
using MIDCloud.API.Models.ResponseModels;
using MIDCloud.API.Services.Interfaces;
using System.IO;
using System.Text;

namespace MIDCloud.API.Services
{
    public class FileService : IFileService
    {
        public Result<TilesOfFolder> GetTiles(string folderPath)
        {
            try
            {
                var resultFolder = GetFilesAndEmptyDirectories(folderPath);//GetFilesAndDirectories(folderPath, resultFolder); this is for full folder elements

                return Result.Success(resultFolder);
            }
            catch (Exception ex)
            {
                return Result.Error($"{ex.Message}");
            }
        }

        public Result<TilesOfFolder> UploadFiles(List<IFormFile> files, string storagePath)
        {
            List<string> erroredFiles = new List<string>();

            foreach (var file in files)
            {
                var result = UploadFile(file, storagePath);

                if (result.IsSuccess is false)
                {
                    erroredFiles.Add(result.Errors.FirstOrDefault());
                }
            }

            if (erroredFiles.Count > 0)
            {
                var errorText = new StringBuilder();
                foreach (var error in erroredFiles)
                {
                    errorText.AppendLine(error);
                }

                return Result.Error(errorText.ToString());
            }

            return Result.Success(GetTiles(storagePath));
        }

        public Result<DirectoryInfo> CreateFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                return Result.Error("Directory already exist.");
            }

            try
            {
                var result = Directory.CreateDirectory(folderPath);

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public Result DeleteFolder(string folderPath)
        {
            if (Directory.Exists(folderPath) is false)
            {
                return Result.Error("Directory already deleted.");
            }

            try
            {
                Directory.Delete(folderPath, true);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        private Result UploadFile(IFormFile file, string storagePath)
        {
            if (file == null || file.Length == 0)
            {
                return Result.Error("File is empty");
            }

            try
            {
                var fileName = Path.GetFileName(file.FileName);

                var filePath = Path.Combine(storagePath, fileName);

                if (System.IO.File.Exists(filePath))
                {
                    return Result.Error("File exist");
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        private Folder GetFilesAndDirectories(string folderPath, Folder resultFolder)
        {
            string[] fileEntries = Directory.GetFiles(folderPath);

            foreach (string fileName in fileEntries)
            {
                resultFolder.Files.Add(new Models.FileModels.File(fileName));
            }

            string[] subdirectoryEntries = Directory.GetDirectories(folderPath);

            foreach (string subdirectory in subdirectoryEntries)
            {
                var nextResult = GetFilesAndDirectories(subdirectory, new Folder(subdirectory));
                resultFolder.Folders.Add(nextResult);
            }

            return resultFolder;
        }

        private TilesOfFolder GetFilesAndEmptyDirectories(string folderPath)
        {
            List<string> fileEntries = Directory.GetFiles(folderPath).ToList();
            List<string> subdirectoryEntries = Directory.GetDirectories(folderPath).ToList();

            return new TilesOfFolder(subdirectoryEntries, fileEntries);
        }
    }
}
