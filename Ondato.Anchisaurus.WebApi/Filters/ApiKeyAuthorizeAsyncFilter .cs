using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Ondato.Anchisaurus.Bll.Services;
using Ondato.Anchisaurus.Core.Models.Constants;
using Ondato.Anchisaurus.WebApi.Models.ActionResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ondato.Anchisaurus.WebApi.Filters
{
    public class ApiKeyAuthorizeAsyncFilter : IAsyncAuthorizationFilter
    {
        private readonly ILogger<ApiKeyAuthorizeAsyncFilter> logger;
        private readonly IApiKeyService apiKeyService;

        public ApiKeyAuthorizeAsyncFilter(ILogger<ApiKeyAuthorizeAsyncFilter> logger, IApiKeyService apiKeyService)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (apiKeyService == null)
                throw new ArgumentNullException(nameof(apiKeyService));

            this.logger = logger;
            this.apiKeyService = apiKeyService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.ActionDescriptor.EndpointMetadata.Any(em => em.GetType() == typeof(AllowAnonymousAttribute)))
                return;

            var request = context.HttpContext.Request;
            var hasApiKeyHeader = request.Headers.TryGetValue(ApiKeyConstants.HeaderName, out var apiKeyValue);

            if (!hasApiKeyHeader)
            {
                context.Result = new CustomUnauthorizedResult($"{ApiKeyConstants.HeaderName} header not found");

                return;
            }

            if (apiKeyValue.Count == 0 || string.IsNullOrEmpty(apiKeyValue))
            {
                context.Result = new CustomUnauthorizedResult($"{ApiKeyConstants.HeaderName} header is empty");

                return;
            }

            if (await apiKeyService.IsAuthorizedAsync(apiKeyValue))
            {
                var principal = CreatePrincipal(apiKeyValue);

                context.HttpContext.User = principal;

                return;
            }

            logger.LogError("API key \"{ApiKeyValue}\" is not valid", apiKeyValue);

            context.Result = new CustomUnauthorizedResult("Unauthorized");
        }

        private ClaimsPrincipal CreatePrincipal(string apiKeyValue)
        {
            var apiKeyClaim = new Claim(ApiKeyConstants.ClaimName, apiKeyValue);
            var clientName = apiKeyService.GetClientName(apiKeyValue);
            var nameClaim = new Claim(ClaimTypes.Name, clientName);

            var principal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                apiKeyClaim,
                nameClaim
            }, ApiKeyConstants.AuthenticationType));

            return principal;
        }
    }
}