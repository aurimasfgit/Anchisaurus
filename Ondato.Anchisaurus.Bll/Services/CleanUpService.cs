using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ondato.Anchisaurus.Core.Interfaces;
using Ondato.Anchisaurus.Core.Models.Settings;
using Ondato.Anchisaurus.Core.Utilities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ondato.Anchisaurus.Bll.Services
{
    public class CleanUpService : IHostedService, IDisposable
    {
        private readonly ILogger<CleanUpService> logger;
        private readonly CleanUpOptions cleanUpOptions;
        private readonly IServiceScopeFactory serviceScopeFactory;

        private ActionRepeater actionRepeater;

        public CleanUpService(ILogger<CleanUpService> logger, IOptions<CleanUpOptions> cleanUpOptions, IServiceScopeFactory serviceScopeFactory)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (cleanUpOptions == null)
                throw new ArgumentNullException(nameof(cleanUpOptions));

            if (serviceScopeFactory == null)
                throw new ArgumentNullException(nameof(serviceScopeFactory));

            this.logger = logger;
            this.cleanUpOptions = cleanUpOptions.Value;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("CleanUpService started");

            if (cleanUpOptions == null)
                throw new ArgumentNullException(nameof(cleanUpOptions));

            if (cleanUpOptions.IntervalInSeconds < 0)
                throw new ArgumentException("Interval cannot be less than zero");

            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var cleanable = serviceScope.ServiceProvider.GetRequiredService<ICleanable>();

                if (cleanable == null)
                    throw new Exception("Failed to get ICleanable service");

                actionRepeater = new ActionRepeater(cleanUpOptions.IntervalInSeconds, cleanable.CleanUp);
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("CleanUpService stopped");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            actionRepeater?.Dispose();
        }
    }
}