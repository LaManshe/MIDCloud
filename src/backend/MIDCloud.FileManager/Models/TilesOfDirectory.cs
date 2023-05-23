using Microsoft.AspNetCore.Mvc;
using MIDCloud.FileManager.Models.Interfaces;
using MIDCloud.GlobalInterfaces.FileSystem;
using MIDCloud.GlobalInterfaces.Requests;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace MIDCloud.FileManager.Models
{
    internal class TilesOfDirectory : ITiles
    {
        public List<IFolder> Folders { get; set; }
        public List<IFile> Files { get; set; }

        public TilesOfDirectory(List<string> folders, List<string> files)
        {
            Folders = new List<IFolder>();
            Files = new List<IFile>();

            foreach (var folder in folders)
            {
                Folders.Add(new Folder(folder));
            }
            foreach (var file in files)
            {
                Files.Add(new CompressedFile(file));
            }
        }

        public void SortFiles(SortFileTypeEnum sortType)
        {
            switch (sortType)
            {
                case SortFileTypeEnum.ByUpload:
                    Files.Sort((x, y) => DateTime.Compare(y.UploadTime, x.UploadTime));
                    break;
                case SortFileTypeEnum.ByCreation:
                    Files = SortFilesByCreationTime();
                    break;
            }
        }

        private List<IFile> SortFilesByCreationTime()
        {
            var result = new List<IFile>();
            
            var listFilesWhoHasCreationTime = Files
                .Where(x => x.Extension is IExtensionHasCreationTime)
                .ToList();

            listFilesWhoHasCreationTime.Sort((x, y) =>
                DateTime.Compare(
                    (x.Extension as IExtensionHasCreationTime).CreationTime,
                    (y.Extension as IExtensionHasCreationTime).CreationTime));

            var listFilesWhoHasntCreationTime = Files
                .Except(listFilesWhoHasCreationTime)
                .ToList();
            
            result.AddRange(listFilesWhoHasCreationTime);
            result.AddRange(listFilesWhoHasntCreationTime);

            return result;
        }
    }
}
