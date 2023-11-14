namespace ProjectOversight.API.ServiceFacade.Response
{
    public class ApiResultResponse
    {
        public ApiResultResponse()
        {
        }
        public ApiResultResponse(dynamic result, string message)
        {
            StatusCode = 200;
            Result = result;
            Message = message;
        }

        public ApiResultResponse(int status, dynamic result, string message)
        {
            StatusCode = status;
            Result = result;
            Message = message;
        }

        public int StatusCode { get; set; }
        public dynamic Result { get; set; }
        public string Message { get; set; }
    }
}
