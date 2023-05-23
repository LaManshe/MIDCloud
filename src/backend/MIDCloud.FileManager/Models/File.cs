using MIDCloud.GlobalInterfaces.FileSystem;
using MIDCloud.GlobalInterfaces.FileSystem.FileExtensions.Base;

namespace MIDCloud.FileManager.Models
{
    internal class File : IFile
    {
        private readonly string[] imageExtensions = { ".jpg", ".jpeg", ".png" };
        private readonly string[] videoExtensions = { ".mp4", ".avi", ".mov", ".wmv", ".flv", ".mkv" };
        private readonly string _path;

        public string Name { get; set; }

        public DateTime UploadTime { get; set; }

        public object Extension { get; }

        public File(string path)
        {
            _path = path;

            Name = Path.GetFileName(_path);
            Extension = GetExtension(_path);
            UploadTime = new FileInfo(_path).CreationTime;
        }

        private object GetExtension(string path)
        {
            return DefineExtensionAsEnum(path) switch
            {
                FileExtensionsTypeEnum.Image => new ImageExtension(path),
                FileExtensionsTypeEnum.Video => new VideoExtension(path),
                FileExtensionsTypeEnum.Unknown => new UnknownExtension(path),
                _ => new UnknownExtension(path),
            };
        }

        private FileExtensionsTypeEnum DefineExtensionAsEnum(string path)
        {
            if (HasImageExtension(path))
            {
                return FileExtensionsTypeEnum.Image;
            }

            if (HasVideoExtension(path))
            {
                return FileExtensionsTypeEnum.Video;
            }

            return FileExtensionsTypeEnum.Unknown;
        }

        private bool HasVideoExtension(string file)
        {
            return videoExtensions.Contains(Path.GetExtension(file).ToLower());
        }

        private bool HasImageExtension(string file)
        {
            return imageExtensions.Contains(Path.GetExtension(file).ToLower());
        }
    }
}
