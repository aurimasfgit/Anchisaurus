using Microsoft.Extensions.Options;
using Ondato.Anchisaurus.Core.Interfaces;
using Ondato.Anchisaurus.Core.Interfaces.Services;
using Ondato.Anchisaurus.Core.Models.Settings;
using Ondato.Anchisaurus.Dal.Repositories;
using System;
using System.Collections.Generic;

namespace Ondato.Anchisaurus.Bll.Services
{
    public class DbKeyValuePairWithExpirationService : IKeyValuePairWithExpirationService, ICleanable
    {
        private readonly KeyValuePairWithExpirationOptions keyValuePairWithExpirationOptions;
        private readonly IKeyValuePairWithExpirationRepository keyValuePairWithExpirationRepository;

        public DbKeyValuePairWithExpirationService(
            IOptions<KeyValuePairWithExpirationOptions> keyValuePairWithExpirationOptions,
            IKeyValuePairWithExpirationRepository keyValuePairWithExpirationRepository)
        {
            if (keyValuePairWithExpirationOptions == null)
                throw new ArgumentNullException(nameof(keyValuePairWithExpirationOptions));

            if (keyValuePairWithExpirationRepository == null)
                throw new ArgumentNullException(nameof(keyValuePairWithExpirationRepository));

            this.keyValuePairWithExpirationOptions = keyValuePairWithExpirationOptions.Value;
            this.keyValuePairWithExpirationRepository = keyValuePairWithExpirationRepository;
        }

        public void CleanUp()
        {
            // [here we will delete all the expired records from repository]
        }

        public void AddOrUpdate(string key, IList<object> value, int? expirationTimeInSeconds = null)
        {
            throw new NotImplementedException();
        }

        public void AddOrAppend(string key, IList<object> value)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            throw new NotImplementedException();
        }

        public IList<object> Get(string key)
        {
            throw new NotImplementedException();
        }
    }
}