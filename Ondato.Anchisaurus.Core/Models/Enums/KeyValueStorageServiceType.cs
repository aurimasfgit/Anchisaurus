namespace Ondato.Anchisaurus.Core.Models.Enums
{
    public enum KeyValueStorageServiceType : int
    {
        /// <summary>
        /// InMemoryKeyValuePairWithExpirationService
        /// </summary>
        InMemory = 1,

        /// <summary>
        /// DbKeyValuePairWithExpirationService
        /// </summary>
        Database = 2
    }
}