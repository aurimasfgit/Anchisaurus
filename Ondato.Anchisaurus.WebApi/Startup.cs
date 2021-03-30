using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Ondato.Anchisaurus.WebApi.Filters;
using Ondato.Anchisaurus.Core.Models.Constants;
using Ondato.Anchisaurus.WebApi.Extensions.DependencyInjection;

namespace Ondato.Anchisaurus.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMvc(setup =>
            {
                setup.Filters.Add<ApiKeyAuthorizeAsyncFilter>();
                setup.Filters.Add<CustomExceptionFilter>();
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Anchisaurus Web API", Version = "v1" });

                options.AddSecurityDefinition(ApiKeyConstants.Scheme, new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = ApiKeyConstants.HeaderName,
                    Type = SecuritySchemeType.ApiKey
                });

                options.OperationFilter<SwaggerSecurityRequirementsFilter>();
            });

            services.InjectDependencies(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Anchisaurus Web API v1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}