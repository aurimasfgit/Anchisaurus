using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Ondato.Anchisaurus.Core.Models.Constants;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Ondato.Anchisaurus.WebApi.Filters
{
    public class SwaggerSecurityRequirementsFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!context.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any())
            {
                if (operation.Security == null)
                    operation.Security = new List<OpenApiSecurityRequirement>();

                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = ApiKeyConstants.Scheme
                            }
                        },
                        new string[]{ }
                    }
                });
            }
        }
    }
}