using MIDCloud.GlobalInterfaces.Requests;

namespace MIDCloud.API.Helpers;

public static class StringToEnumConverter
{
    public static SortFileTypeEnum GetSortFileType(string value)
    {
        switch (value)
        {
            case "ByUpload": return SortFileTypeEnum.ByUpload;
            case "ByCreation": return SortFileTypeEnum.ByCreation;
            default: return SortFileTypeEnum.Unknown;
        }
    }
}