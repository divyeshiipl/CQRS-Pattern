namespace Infrastructure.Services.Common;

public class ApiClientService : IApiClientService
{
    private readonly HttpClient _httpClient;
    private readonly IDistributedCache _distributedCache;
    private readonly ISeriLogAppService _seriLogAppService;

    public ApiClientService(HttpClient httpClient, IDistributedCache distributedCache, ISeriLogAppService seriLogAppService)
    {
        _httpClient = httpClient;
        _distributedCache = distributedCache;
        _seriLogAppService = seriLogAppService;
    }

    public async Task<HttpResponseMessage> PostAsync<T>(string url, T data, string? bearerToken = null)
    {
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        };

        string json = JsonSerializer.Serialize(data, options);

        var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

        _httpClient.DefaultRequestHeaders.Clear();

        if (!string.IsNullOrWhiteSpace(bearerToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        }

        var response = await _httpClient.PostAsync(url, jsonContent).ConfigureAwait(false);

        return response;
    }

    public async Task<HttpResponseMessage> GetAsync<T>(string url, T data, string? accessToken = null)
    {
        var queryString = ToQueryString(data);
        try
        {
            
            var finalUrl = string.IsNullOrWhiteSpace(queryString) ? url : $"{url}?{queryString}";

            _httpClient.DefaultRequestHeaders.Clear();

            if (!string.IsNullOrEmpty(accessToken))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            }

            var response = await _httpClient.GetAsync(finalUrl).ConfigureAwait(false);

            await _seriLogAppService.LogExternalApiAuditLogs(url, "GetAsync", queryString, response, "VehiclePolicies", "GET");
            return response;
        }
        catch (Exception ex)
        {
            await _seriLogAppService.LogExternalApiAuditLogs(url, "GetAsync", queryString, null, ex.Message, "GET");
            throw;
        }
    }

    public async Task<string> GetAccessToken(IdentityTokenRequestDto tokenRequestDto)
    {
        var RequestBody = new Dictionary<string, string>();

        string tokenUrl = tokenRequestDto.TokenEndPoint;
        string client_id = tokenRequestDto.ClientId;
        string client_secret = tokenRequestDto.ClientSecret;
        string grant_type = tokenRequestDto.GrantType;

        var cacheData = await _distributedCache.GetStringAsync(client_id);
        if (!string.IsNullOrEmpty(cacheData))
        {
            return cacheData;
        }
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

        RequestBody.Add(Constants.strClientId, client_id);
        RequestBody.Add(Constants.strClientSecret, client_secret);
        RequestBody.Add(Constants.strGrantType, grant_type);

        HttpResponseMessage response = await _httpClient.PostAsync(tokenUrl, new FormUrlEncodedContent(RequestBody));

        string jsonRequestString = JsonSerializer.Serialize(RequestBody);
        await _seriLogAppService.LogExternalApiAuditLogs(tokenUrl, "GetAccessToken", jsonRequestString, response, "External API Call");

        if (response != null && response.IsSuccessStatusCode)
        {
            var JsonContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<IdentityTokenResponseDto>(JsonContent);

            DistributedCacheEntryOptions distributedCacheEntryOptions = new DistributedCacheEntryOptions();
            distributedCacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddSeconds(Convert.ToInt32(result.expires_in) - tokenRequestDto.TokenExpiryInSecond);
            await _distributedCache.SetStringAsync(client_id, result.access_token, distributedCacheEntryOptions);
            return result.access_token;
        }
        return string.Empty;
    }

    private string ToQueryString<T>(T obj)
    {
        if (obj == null) return string.Empty;

        var properties = typeof(T).GetProperties()
            .Where(p => p.GetValue(obj) != null)
            .Select(p => $"{Uri.EscapeDataString(p.Name)}={Uri.EscapeDataString(p.GetValue(obj)?.ToString() ?? "")}");

        return string.Join("&", properties);
    }    
}
