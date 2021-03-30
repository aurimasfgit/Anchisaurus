using System;
using System.Collections.Generic;

namespace Ondato.Anchisaurus.Core.Models
{
    public class KeyValuePairStorageValue
    {
        public IList<object> Value { get; }

        public bool IsExpired()
        {
            return (DateTime.UtcNow - dateTimeAdded) > timeToLive;
        }

        public void ResetExpiration()
        {
            dateTimeAdded = DateTime.UtcNow;
        }

        private DateTime dateTimeAdded;
        private readonly TimeSpan timeToLive;

        public KeyValuePairStorageValue(IList<object> value, int expirationTimeInSeconds)
        {
            Value = value;

            dateTimeAdded = DateTime.UtcNow;
            timeToLive = TimeSpan.FromSeconds(expirationTimeInSeconds);
        }
    }
}