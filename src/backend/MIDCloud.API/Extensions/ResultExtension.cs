using Ardalis.Result;
using System.Text;

namespace MIDCloud.API.Extensions
{
    public static class ResultExtension
    {
        public static string ConcatErrors<T>(this Result<T> result, string[]? args = default)
        {
            if (result.Errors.Count() < 2 || args is null)
            {
                return result.Errors.FirstOrDefault();
            }

            var errorsText = new StringBuilder();

            foreach (var error in result.Errors)
            {
                errorsText.AppendLine(error);
            }

            foreach (var error in args)
            {
                errorsText.AppendLine(error);
            }

            return errorsText.ToString();
        }
    }
}
