using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Ondato.Anchisaurus.WebApi.Models.ActionResults;

namespace Ondato.Anchisaurus.WebApi.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled)
                return;

            // Custom exception handling. Pretty simple for now :]
            if (context.Exception != null && !string.IsNullOrEmpty(context.Exception.Message))
            {
                context.Result = new CustomResult(StatusCodes.Status500InternalServerError, context.Exception.Message);
            }
        }
    }
}