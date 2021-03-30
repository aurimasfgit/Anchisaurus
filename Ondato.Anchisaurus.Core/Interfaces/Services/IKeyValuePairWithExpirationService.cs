using System.Collections.Generic;

namespace Ondato.Anchisaurus.Core.Interfaces.Services
{
    public interface IKeyValuePairWithExpirationService
    {
        void AddOrUpdate(string key, IList<object> value, int? expirationTimeInSeconds = null);
        void AddOrAppend(string key, IList<object> value);
        void Remove(string key);

        IList<object> Get(string key);
    }
}