namespace Domain.Common;

public class IdentityTokenRequestDto
{
    public required string TokenEndPoint { get; set; }

    public required string ClientId { get; set; }

    public required string ClientSecret { get; set; }

    public required string GrantType { get; set; }
    public  string? Scope { get; set; }

    public required int TokenExpiryInSecond { get; set; }
}
