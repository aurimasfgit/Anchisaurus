namespace Ondato.Anchisaurus.Core.Models.Settings
{
    public class KeyValuePairWithExpirationOptions
    {
        public int DefaultExpirationTimeInSeconds { get; set; }
        public int MaxExpirationTimeInSeconds { get; set; }
    }
}