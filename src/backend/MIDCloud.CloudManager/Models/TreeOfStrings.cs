using MIDCloud.GlobalInterfaces.Responses;

namespace MIDCloud.CloudManager.Models;

public class TreeOfStrings: IBranch<string>
{
    public string Name { get; set; }
    
    public List<IBranch<string>> Branches { get; set; }

    public TreeOfStrings(string startPath, IEnumerable<string> paths)
    {
        Name = Path.GetFileName(startPath) ?? "Unknown";

        if (IsNotFile(Name))
        {
            Branches = new List<IBranch<string>>();
        
            Branches.AddRange(GetChildrenOfDirectory(startPath, paths));
        }
    }

    private bool IsNotFile(string name)
    {
        return Path.GetExtension(name) == "";
    }

    private List<IBranch<string>> GetChildrenOfDirectory(string startPath, IEnumerable<string> paths)
    {
        List<IBranch<string>> result = new List<IBranch<string>>();
        
        var children = paths
            .Where(path => Path.GetDirectoryName(path) == startPath).ToArray();

        foreach (var child in children)
        {
            result.Add(new TreeOfStrings(child, paths));
        }

        return result;
    }
}