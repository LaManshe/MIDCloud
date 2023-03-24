using MIDCloud.API.Models.ResponseModels.Interface;

namespace MIDCloud.API.Models.ResponseModels
{
    public class FileTile : IFile
    {
        public string Name { get; set; }

        public FileTile(string name)
        {
            Name = name;
        }
    }
}
