using Ardalis.GuardClauses;
using MIDCloud.API.Models.ResponseModels.Interface;

namespace MIDCloud.API.Models.ResponseModels
{
    public class ResponseSuccessModel : IResponseModel
    {
        public string Message { get; set; }

        public ResponseSuccessModel(string message)
        {
            Message = Guard.Against.NullOrEmpty(message, nameof(message));
        }
    }
}
