using Microsoft.Extensions.Options;
using Ondato.Anchisaurus.Core.Interfaces;
using Ondato.Anchisaurus.Core.Interfaces.Services;
using Ondato.Anchisaurus.Core.Models;
using Ondato.Anchisaurus.Core.Models.Settings;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Ondato.Anchisaurus.Bll.Services
{
    public class InMemoryKeyValuePairWithExpirationService : IKeyValuePairWithExpirationService, ICleanable
    {
        private readonly ConcurrentDictionary<string, KeyValuePairStorageValue> dictionary = new ConcurrentDictionary<string, KeyValuePairStorageValue>();

        private readonly KeyValuePairWithExpirationOptions keyValuePairWithExpirationOptions;

        public InMemoryKeyValuePairWithExpirationService(IOptions<KeyValuePairWithExpirationOptions> keyValuePairWithExpirationOptions)
        {
            if (keyValuePairWithExpirationOptions == null)
                throw new ArgumentNullException(nameof(keyValuePairWithExpirationOptions));

            this.keyValuePairWithExpirationOptions = keyValuePairWithExpirationOptions.Value;
        }

        private readonly object cleanUpLocker = new object();

        public void CleanUp()
        {
            lock (cleanUpLocker)
            {
                foreach (var item in dictionary)
                {
                    if (item.Value.IsExpired())
                        dictionary.TryRemove(item.Key, out _);
                }
            }
        }

        private readonly object appendLocker = new object();

        public void AddOrAppend(string key, IList<object> value)
        {
            if (!dictionary.TryAdd(key, new KeyValuePairStorageValue(value, keyValuePairWithExpirationOptions.DefaultExpirationTimeInSeconds)))
            {
                lock (appendLocker)
                {
                    foreach (var item in value)
                        dictionary[key].Value.Add(item);
                }
            }
        }

        public void AddOrUpdate(string key, IList<object> value, int? expirationTimeInSeconds = null)
        {
            expirationTimeInSeconds = expirationTimeInSeconds ?? keyValuePairWithExpirationOptions.DefaultExpirationTimeInSeconds;

            if (expirationTimeInSeconds > keyValuePairWithExpirationOptions.MaxExpirationTimeInSeconds)
                throw new ArgumentException($"Expiration time cannot be greater than {keyValuePairWithExpirationOptions.MaxExpirationTimeInSeconds} seconds");

            var storageValue = new KeyValuePairStorageValue(value, expirationTimeInSeconds.Value);

            dictionary.AddOrUpdate(key, storageValue, (oldkey, oldvalue) => storageValue);
        }

        public void Remove(string key)
        {
            if (!dictionary.TryRemove(key, out _))
                throw new Exception($"Storage does not contains item with key \"{key}\"");
        }

        public IList<object> Get(string key)
        {
            if (dictionary.TryGetValue(key, out var storageValue))
            {
                storageValue.ResetExpiration();

                return storageValue.Value;
            }
            else
                throw new Exception($"Storage does not contains item with key \"{key}\"");
        }
    }
}