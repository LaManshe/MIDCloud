namespace MIDCloud.API.Models
{
    public enum UploadStatusEnum
    {
        Success,
        Failed
    }

    public class UploadStatus
    {
        public List<UploadFileStatus> Files { get; set; } = new List<UploadFileStatus>();
        public bool IsFailed => Files.Any(x => x.Status == UploadStatusEnum.Failed);
    }

    public class UploadFileStatus
    {
        public DateTime Date { get; set; }
        public string FileName { get; set; }
        public UploadStatusEnum Status { get; set; }
        public string Message { get; set; }
    }
}
