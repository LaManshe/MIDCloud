using MIDCloud.API.Models.ResponseModels.Interface;

namespace MIDCloud.API.Models.ResponseModels
{
    public class FolderTile : IFolder
    {
        public string Name { get; set; }

        public FolderTile(string name)
        {
            Name = name;
        }
    }
}
