using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ondato.Anchisaurus.Bll.Services
{
    public interface IApiKeyService
    {
        Task<bool> IsAuthorizedAsync(string apiKey);
        string GetClientName(string apiKey);
    }

    public class ApiKeyService : IApiKeyService
    {
        /// <summary>
        /// API keys and clients are hard-coded for now.
        /// In the future need to load them from persistent storage (DB, etc.)
        /// </summary>
        private readonly IDictionary<string, string> apiKeyClientMap = new Dictionary<string, string>
        {
            { "123456789", "Client1"},
            { "55555", "Client2" },
            { "123", "Client3" }
        };

        /// <summary>
        /// Checks if the API KEY is authorized
        /// </summary>
        /// <param name="apiKey">API KEY to validate</param>
        /// <returns>true if API KEY is valid; otherwise false</returns>
        public async Task<bool> IsAuthorizedAsync(string apiKey)
        {
            return await new ValueTask<bool>(apiKeyClientMap.ContainsKey(apiKey));
        }

        /// <summary>
        /// Returns name of the client to which the API KEY was issued
        /// </summary>
        /// <param name="apiKey">Issued API KEY</param>
        /// <returns>Name of the client</returns>
        public string GetClientName(string apiKey)
        {
            if (!apiKeyClientMap.ContainsKey(apiKey))
                throw new KeyNotFoundException($"ApiKeyClientMap does not contains key \"{apiKey}\"");

            return apiKeyClientMap[apiKey];
        }
    }
}