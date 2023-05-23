using Microsoft.AspNetCore.Mvc;
using MIDCloud.GlobalInterfaces.FileSystem.FileExtensions.Base;

namespace MIDCloud.FileManager.Models;

public class VideoExtension: VideoCompressedExtension
{
    public override string Type => FileExtensionsTypeEnum.Video.ToString();
    
    public FileStreamResult? VideoStream { get; set; }

    public VideoExtension(string path) : base(path)
    {
        VideoStream = GetVideoStreamFromFile(path);
    }

    private FileStreamResult? GetVideoStreamFromFile(string file)
    {
        var stream = new FileStream(file, FileMode.Open);
        
        return new FileStreamResult(stream, "video/mp4");
    }
}