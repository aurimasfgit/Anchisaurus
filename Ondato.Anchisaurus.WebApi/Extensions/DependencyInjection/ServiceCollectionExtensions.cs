using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ondato.Anchisaurus.Bll.Services;
using Ondato.Anchisaurus.Core.Interfaces;
using Ondato.Anchisaurus.Core.Interfaces.Services;
using Ondato.Anchisaurus.Core.Models.Enums;
using Ondato.Anchisaurus.Core.Models.Settings;
using Ondato.Anchisaurus.Dal;
using Ondato.Anchisaurus.Dal.Context;
using Ondato.Anchisaurus.Dal.Repositories;
using System;

namespace Ondato.Anchisaurus.WebApi.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void InjectDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptionsFromConfig(configuration);
            services.AddLiteDB(configuration);
            services.AddScoped<ITransaction, Transaction>();
            services.AddScoped<IApiKeyService, ApiKeyService>();

            services.AddHostedService<CleanUpService>();

            services.AddKeyValuePairWithExpirationStorage(configuration);
        }

        public static void AddKeyValuePairWithExpirationStorage(this IServiceCollection services, IConfiguration configuration)
        {
            var keyValuePairServiceType = configuration.GetValue<KeyValueStorageServiceType>("KeyValuePairServiceType");

            if (keyValuePairServiceType == KeyValueStorageServiceType.InMemory)
                services.AddInMemoryKeyValuePairWithExpirationStorage();
            else if (keyValuePairServiceType == KeyValueStorageServiceType.Database)
                services.AddDbKeyValuePairWithExpirationStorage();
            else
                throw new ArgumentException("Unknown KeyValuePairServiceType value");
        }

        public static void AddInMemoryKeyValuePairWithExpirationStorage(this IServiceCollection services)
        {
            services.AddSingleton<InMemoryKeyValuePairWithExpirationService>();
            services.AddSingleton<ICleanable>(x => x.GetRequiredService<InMemoryKeyValuePairWithExpirationService>());
            services.AddSingleton<IKeyValuePairWithExpirationService>(x => x.GetRequiredService<InMemoryKeyValuePairWithExpirationService>());
        }

        public static void AddDbKeyValuePairWithExpirationStorage(this IServiceCollection services)
        {
            services.AddScoped<ICleanable, DbKeyValuePairWithExpirationService>();
            services.AddScoped<IKeyValuePairWithExpirationService, DbKeyValuePairWithExpirationService>();

            services.AddScoped<IKeyValuePairWithExpirationRepository, KeyValuePairWithExpirationRepository>();
        }

        public static void AddOptionsFromConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .Configure<KeyValuePairWithExpirationOptions>(configuration.GetSection("KeyValuePairWithExpiration"))
                .PostConfigure<KeyValuePairWithExpirationOptions>(options =>
                {
                    if (options.DefaultExpirationTimeInSeconds < 0)
                        throw new ArgumentException("KeyValuePairWithExpiration:DefaultExpirationTimeInSeconds cannot be less than zero");

                    if (options.MaxExpirationTimeInSeconds < 0)
                        throw new ArgumentException("KeyValuePairWithExpiration:MaxExpirationTimeInSeconds cannot be less than zero");

                    if (options.DefaultExpirationTimeInSeconds > options.MaxExpirationTimeInSeconds)
                        throw new ArgumentException("KeyValuePairWithExpiration:DefaultExpirationTimeInSeconds cannot be greater than KeyValuePairWithExpiration:MaxExpirationTimeInSeconds");
                });

            services.Configure<CleanUpOptions>(configuration.GetSection("CleanUp"));
        }

        public static void AddLiteDB(this IServiceCollection services, IConfiguration configuration)
        {
            var liteDbConnectionString = configuration.GetConnectionString("LiteDB");

            if (string.IsNullOrEmpty(liteDbConnectionString))
                throw new ArgumentNullException("ConnectionStrings:LiteDB");

            services.AddScoped<LiteDbContext, LiteDbContext>();
            services.Configure<LiteDbOptions>(options => options.ConnectionString = liteDbConnectionString);
        }
    }
}