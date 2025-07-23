namespace Domain.Common
{
    public class LDAPIConfiguration
    {
        public const string Name = "LDAPI:APISettings";

        public required string Url { get; set; }

        public required string ClientId { get; set; }

        public required string ClientSecret { get; set; }
    }
}
