namespace Domain.Common;

public class Constants
{
    #region IAM

    public const string appIdHeader = "app-id";
    public const string appKeyHeader = "x-api-key";
    public const string strClientId = "client_id";//This is static header key not a value and Value is coming from the secure source with encryption.
    public const string strClientSecret = "client_secret";//This is static header key not a value and Value is coming from the secure source with encryption.
    public const string strGrantType = "grant_type";
    public const string strHeader = "application/json";
    public const string strClientCredentials = "client_credentials";

    #endregion
}
