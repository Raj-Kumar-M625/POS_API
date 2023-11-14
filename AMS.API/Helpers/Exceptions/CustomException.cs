namespace ProjectOversight.API.Helper.Exceptions
{
    public class CustomException : Exception
    {
        public CustomException()
        {
        }

        public CustomException(string message) : base(message)
        {
        }

        public CustomException(string message, string responseModel) : base(message)
        {
        }

        public CustomException(string message, CustomException innerException) : base(message, innerException)
        {
        }
    }
}
