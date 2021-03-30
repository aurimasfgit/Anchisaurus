using Microsoft.AspNetCore.Http;

namespace Ondato.Anchisaurus.WebApi.Models.ActionResults
{
    public class CustomUnauthorizedResult : CustomResult
    {
        public CustomUnauthorizedResult(string message)
            : base(StatusCodes.Status401Unauthorized, message)
        {
        }
    }
}