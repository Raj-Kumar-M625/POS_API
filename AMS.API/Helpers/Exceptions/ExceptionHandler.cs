
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.Net;
using System.Reflection;

namespace ProjectOversight.API.Helper.Exceptions
{
    public class ExceptionHandler : IExceptionFilter
    {
        /// <summary>
        /// Purpose = To Handle Exception based on the Exception Occurred
        /// Author = Manoj BJ
        /// Date = 20-07-2022
        /// </summary>
        public enum Exceptions
        {
            NullReferenceException = 1,
            FileNotFoundException = 2,
            OverflowException = 3,
            OutOfMemoryException = 4,
            InvalidCastException = 5,
            ObjectDisposedException = 6,
            UnauthorizedAccessException = 7,
            NotImplementedException = 8,
            NotSupportedException = 9,
            InvalidOperationException = 10,
            TimeoutException = 11,
            ArgumentException = 12,
            FormatException = 13,
            StackOverflowException = 14,
            SqlException = 15,
            IndexOutOfRangeException = 16,
            IOException = 17
        }
        private HttpStatusCode getErrorCode(Type exceptionType)
        {
            Exceptions tryParseResult;
            if (Enum.TryParse<Exceptions>(exceptionType.Name, out tryParseResult))
            {
                switch (tryParseResult)
                {
                    case Exceptions.NullReferenceException:
                        return HttpStatusCode.LengthRequired;

                    case Exceptions.FileNotFoundException:
                        return HttpStatusCode.NotFound;

                    case Exceptions.OverflowException:
                        return HttpStatusCode.RequestedRangeNotSatisfiable;

                    case Exceptions.OutOfMemoryException:
                        return HttpStatusCode.ExpectationFailed;

                    case Exceptions.InvalidCastException:
                        return HttpStatusCode.PreconditionFailed;

                    case Exceptions.ObjectDisposedException:
                        return HttpStatusCode.Gone;

                    case Exceptions.UnauthorizedAccessException:
                        return HttpStatusCode.Unauthorized;

                    case Exceptions.NotImplementedException:
                        return HttpStatusCode.NotImplemented;

                    case Exceptions.NotSupportedException:
                        return HttpStatusCode.NotAcceptable;

                    case Exceptions.InvalidOperationException:
                        return HttpStatusCode.MethodNotAllowed;

                    case Exceptions.TimeoutException:
                        return HttpStatusCode.RequestTimeout;

                    case Exceptions.ArgumentException:
                        return HttpStatusCode.BadRequest;

                    case Exceptions.StackOverflowException:
                        return HttpStatusCode.RequestedRangeNotSatisfiable;

                    case Exceptions.FormatException:
                        return HttpStatusCode.UnsupportedMediaType;

                    case Exceptions.IOException:
                        return HttpStatusCode.NotFound;

                    case Exceptions.IndexOutOfRangeException:
                        return HttpStatusCode.ExpectationFailed;

                    default:
                        return HttpStatusCode.InternalServerError;
                }
            }
            else
            {
                return HttpStatusCode.InternalServerError;
            }
        }
        /// <summary>
        /// purpose = Any Exception Occures Globally Handled Here and Redirect To Error Page
        /// Author = Manoj BJ
        /// Date = 20-07-2022
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {

            HttpStatusCode statusCode = (context.Exception as WebException != null &&
                        ((HttpWebResponse)(context.Exception as WebException).Response) != null) ?
                         ((HttpWebResponse)(context.Exception as WebException).Response).StatusCode
                         : getErrorCode(context.Exception.GetType());
            string errorMessage = context.Exception.Message;
            string customErrorMessage = context.Exception.Message;
            string stackTrace = context.Exception.StackTrace;



            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)statusCode;
            //response.ContentType = "application/json";
            var msg = JsonConvert.SerializeObject(
                new
                {
                    message = customErrorMessage,
                    isError = true,
                    errorCode = statusCode,
                    //stackTrace = stackTrace,
                    model = "Please try again Later"
                });

            var result = new ViewResult { ViewName = "Error" };
            var modelMetadata = new EmptyModelMetadataProvider();
            result.ViewData = new ViewDataDictionary(
                    modelMetadata, context.ModelState);
            result.ViewData.Add("HandleException",
                    msg);
            context.Result = result;
            context.ExceptionHandled = true;

            
            response.ContentLength = msg.Length;
            if (result != null)
            {
                context.Result = new ViewResult
                {
                    ViewName = "Error",
                };
            }
            
        }
    }
}