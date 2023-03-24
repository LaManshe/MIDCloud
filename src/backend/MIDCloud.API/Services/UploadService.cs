using Ardalis.Result;
using MIDCloud.API.Models;
using MIDCloud.API.Services.Interfaces;
using OneOf;

namespace MIDCloud.API.Services
{
    public class UploadService : IUploadService
    {
        public Result UploadFile(IFormFile file, string storagePath)
        {
            if (file == null || file.Length == 0)
            {
                return Result.Error("File is empty");
            }

            try
            {
                var fileName = Path.GetFileName(file.FileName);

                var filePath = Path.Combine(storagePath, fileName);

                if (File.Exists(filePath))
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

        public UploadStatus UploadFiles(List<IFormFile> files, string storagePath)
        {
            var uploadResult = new UploadStatus();

            foreach (var file in files)
            {
                var result = UploadFile(file, storagePath);

                if (result.IsSuccess is false)
                {
                    uploadResult.Files.Add(new UploadFileStatus()
                    {
                        Date = DateTime.Now,
                        FileName = file.FileName,
                        Status = UploadStatusEnum.Failed,
                        Message = result.Errors.FirstOrDefault()
                    });
                }
            }

            return uploadResult;
        }
    }
}
