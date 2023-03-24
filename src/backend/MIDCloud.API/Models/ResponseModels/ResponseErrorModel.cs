using MIDCloud.API.Models.ResponseModels.Interface;

namespace MIDCloud.API.Models.ResponseModels
{
    public class ResponseErrorModel : IResponseModel
    {
        public StatusEnum Status => StatusEnum.Error;
        public string Message { get; set; }

        public ResponseErrorModel(string message)
        {
            Message = message;
        }
    }
}
