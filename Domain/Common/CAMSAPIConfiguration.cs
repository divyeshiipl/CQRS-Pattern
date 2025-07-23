namespace Domain.Common
{
    public class CAMSAPIConfiguration
    {
        public const string Name = "CAMS:APISettings";

        public required string Url { get; set; }

        public required string ClientId { get; set; }

        public required string ClientSecret { get; set; }
    }
}
