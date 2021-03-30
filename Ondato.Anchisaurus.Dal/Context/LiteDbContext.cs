using LiteDB;
using Microsoft.Extensions.Options;
using Ondato.Anchisaurus.Core.Models.Settings;
using System;

namespace Ondato.Anchisaurus.Dal.Context
{
    public class LiteDbContext : IDisposable
    {
        public LiteDatabase Database { get; }

        public LiteDbContext(IOptions<LiteDbOptions> config)
        {
            ValidateConfig(config);

            var liteDatabase = new LiteDatabase(config.Value.ConnectionString);

            if (liteDatabase == null)
                throw new ArgumentException("Failed to initialize LiteDatabase");

            Database = liteDatabase;
        }

        private void ValidateConfig(IOptions<LiteDbOptions> config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            if (config.Value == null)
                throw new ArgumentNullException(nameof(config.Value));

            if (string.IsNullOrEmpty(config.Value.ConnectionString))
                throw new ArgumentNullException(nameof(config.Value.ConnectionString));
        }

        public void Dispose()
        {
            Database?.Dispose();
        }
    }
}