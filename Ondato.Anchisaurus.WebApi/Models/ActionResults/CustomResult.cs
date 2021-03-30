using Microsoft.AspNetCore.Mvc;

namespace Ondato.Anchisaurus.WebApi.Models.ActionResults
{
    public class CustomResult : JsonResult
    {
        public CustomResult(int statusCode, string message)
            : base(new CustomError(message))
        {
            StatusCode = statusCode;
        }
    }

    public class CustomError
    {
        public string Error { get; }

        public CustomError(string message)
        {
            Error = message;
        }
    }
}