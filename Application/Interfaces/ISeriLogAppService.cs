namespace Application.Interfaces;

public interface ISeriLogAppService
{
    Task<string> FormatRequestAsync(HttpRequest request);

    Task<string> FormatResponseAsync(Stream responseBody);

    void LogInformation(string message, long? messageId = null);

    void LogInformation(LogDto logdto);

    void LogError(LogDto logdto, Exception ex);

    void LogError(string message, Exception ex, long? messageId = null);

    LogDto SetFromContext();

    Task LogExternalApiAuditLogs(string requestUrl, string action, string request, HttpResponseMessage responseData, string message = "", string httpVerb = "POST");
}
