namespace Application.Interfaces;

public interface IApiClientService
{
    Task<HttpResponseMessage> PostAsync<T>(string url, T data, string? bearerToken = null);
    Task<HttpResponseMessage> GetAsync<T>(string url, T data, string? accessToken = null);
    Task<string> GetAccessToken(IdentityTokenRequestDto tokenRequestDto);
}
