namespace Infrastructure.Repositories;

public class SeriLogAppService : ISeriLogAppService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SeriLogAppService(IHttpContextAccessor httpContextAccessor, IConfiguration seriLogConfiguration)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> FormatRequestAsync(HttpRequest request)
    {
        request.EnableBuffering();
        request.Body.Position = 0;

        using var reader = new StreamReader(request.Body, leaveOpen: true);
        var rawRequestBody = await reader.ReadToEndAsync();
        request.Body.Position = 0;

        if (string.IsNullOrWhiteSpace(rawRequestBody))
            return string.Empty;

        try
        {
            using var jsonDoc = JsonDocument.Parse(rawRequestBody);
            return JsonSerializer.Serialize(jsonDoc, new JsonSerializerOptions
            {
                WriteIndented = false
            });
        }
        catch
        {
            return rawRequestBody.Trim(); //If the body isn't valid JSON, return the raw content as-is
        }
    }


    public async Task<string> FormatResponseAsync(Stream responseBody)
    {
        responseBody.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(responseBody, leaveOpen: true);
        var text = await reader.ReadToEndAsync();
        responseBody.Seek(0, SeekOrigin.Begin); // Reset so ASP.NET Core can use it
        return text;
    }

    public void LogInformation(string message, long? messageId = null)
    {
        HttpContext? httpContext = _httpContextAccessor.HttpContext;
        string? ipAddress = string.Empty;
        string? requestURL = string.Empty;
        if (httpContext != null)
        {
            ipAddress = Convert.ToString(httpContext.Connection?.RemoteIpAddress);
            requestURL = GetAbsoluteUrl(httpContext.Request);
        }

        Log.Logger
            .ForContext("Message", message)
            .ForContext("MessageId", messageId)
            .ForContext("IPAddress", ipAddress)
            .ForContext("MachineName", Environment.MachineName.ToString())
            .ForContext("LogLevel", Convert.ToByte(LogLevel.Information))
            .ForContext("LoggerType", LogLevel.Information.ToString())
            .ForContext("RequestURL", requestURL)
            .ForContext("CreatedOn", DateTime.Now)
            .Information(message);
    }

    public void LogInformation(LogDto logdto)
    {
        GetDefaultLogger(logdto)
            .ForContext("LogLevel", Convert.ToByte(LogLevel.Information))
            .ForContext("LoggerType", LogLevel.Information.ToString())
            .Information(logdto.Message ?? string.Empty);
    }

    public void LogError(LogDto logdto, Exception ex)
    {
        GetDefaultLogger(logdto)
             .ForContext("LogLevel", Convert.ToByte(LogLevel.Error))
             .ForContext("LoggerType", LogLevel.Error.ToString())
             .Error(ex, ex.Message);
    }

    public void LogError(string message, Exception ex, long? messageId = null)
    {
        HttpContext? httpContext = _httpContextAccessor.HttpContext;
        string? ipAddress = string.Empty;
        if (httpContext != null)
        {
            ipAddress = Convert.ToString(httpContext.Connection.RemoteIpAddress) ?? string.Empty;
        }

        Log.Logger
            .ForContext("CreatedOn", DateTime.Now)
            .ForContext("IPAddress", ipAddress)
            .ForContext("MachineName", Environment.MachineName.ToString())
            .ForContext("LogLevel", Convert.ToByte(LogLevel.Error))
            .ForContext("LoggerType", LogLevel.Error.ToString())
            .ForContext("MessageId", messageId)
            .Error(ex, message);
    }

    public LogDto SetFromContext()
    {
        LogDto logdto = new LogDto();
        HttpContext? httpContext = _httpContextAccessor.HttpContext;
        string ipAddress = Convert.ToString(httpContext?.Connection.RemoteIpAddress) ?? string.Empty;
        string? action = Convert.ToString(httpContext?.Request.Path) ?? string.Empty;
        UriBuilder uriBuilder = new UriBuilder();
        uriBuilder.Scheme = httpContext?.Request.Scheme;
        uriBuilder.Host = httpContext?.Request.Host.Host;
        uriBuilder.Path = httpContext?.Request.Path.ToString();
        uriBuilder.Query = httpContext?.Request.QueryString.ToString();
        string requestURL = uriBuilder.Uri.AbsoluteUri;
        logdto.Action = action;
        logdto.IPAddress = ipAddress;
        logdto.RequestURL = requestURL;
        try
        {
            //logdto.Message = string.Format("Request {0} {1} {2}", httpContext?.Request.Method, httpContext?.Request.Path, httpContext?.Response.StatusCode.ToString());
            logdto.Message = httpContext?.Request.Path;
        }
        catch
        {
            logdto.Message = "";
        }
        return logdto;
    }

    private Serilog.ILogger GetDefaultLogger(LogDto? logdto = null)
    {
        string? ipAddress = string.Empty;
        string? action = string.Empty;
        string? requestURL = string.Empty;
        int? statusCode = null;
        string? verb = string.Empty;
        string? matchine = Environment.MachineName.ToString();
        string? request = string.Empty;
        string? response = string.Empty;
        DateTime createdOn = DateTime.Now;
        HttpContext? httpContext = _httpContextAccessor.HttpContext;

        if (httpContext != null)
        {
            ipAddress = Convert.ToString(httpContext.Connection.RemoteIpAddress) ?? string.Empty;
            action = httpContext.Request.Path;
            requestURL = GetAbsoluteUrl(httpContext.Request);
            statusCode = httpContext.Response.StatusCode;
            verb = httpContext.Request.Method;
            request = FormatRequestAsync(httpContext.Request).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        if (logdto != null)
        {
            requestURL = logdto.RequestURL;
            ipAddress = logdto.IPAddress;
            action = logdto.Action;
            request = logdto.Request;
            response = logdto.Response;
            if (logdto.StatusCode != null)
            {
                statusCode = logdto.StatusCode;
            }
        }

        return Log.Logger
                .ForContext("RequestURL", requestURL)
                .ForContext("IPAddress", ipAddress)
                .ForContext("Action", action)
                .ForContext("Request", request)
                .ForContext("Response", response)
                .ForContext("StatusCode", statusCode)
                .ForContext("HttpVerb", verb)
                .ForContext("MachineName", matchine)
                .ForContext("CreatedOn", createdOn);
    }

    private string GetAbsoluteUrl(HttpRequest httpRequest)
    {
        UriBuilder uriBuilder = new UriBuilder();
        uriBuilder.Scheme = httpRequest.Scheme;
        uriBuilder.Host = httpRequest.Host.Host;
        uriBuilder.Path = httpRequest.Path.ToString();
        uriBuilder.Query = httpRequest.QueryString.ToString();
        return uriBuilder.Uri.AbsoluteUri;
    }

    public async Task LogExternalApiAuditLogs(string requestUrl, string action, string request, HttpResponseMessage responseData, string message = "", string httpVerb = "POST")
    {
        var statusCode = (int)responseData.StatusCode;
        var response = await responseData.Content.ReadAsStringAsync();
        var createdOn = DateTime.Now;
        var machineName = Environment.MachineName;
        string ipAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? NetworkUtils.GetServerIpAddress();
        string requestId = _httpContextAccessor.HttpContext?.Request?.Headers["X-Request-ID"].FirstOrDefault() ?? string.Empty;

        Log.Logger
            .ForContext("RequestURL", requestUrl)
            .ForContext("IPAddress", ipAddress)
            .ForContext("RequestId", requestId)
            .ForContext("Action", action)
            .ForContext("Request", request)
            .ForContext("Response", response)
            .ForContext("StatusCode", statusCode)
            .ForContext("HttpVerb", httpVerb)
            .ForContext("MachineName", machineName)
            .ForContext("LogLevel", Convert.ToByte(LogLevel.Information))
            .ForContext("LoggerType", LogLevel.Information.ToString())
            .ForContext("CreatedOn", createdOn)
            .ForContext("CreatedDay", (byte)createdOn.Day)
            .ForContext("CreatedMonth", (byte)createdOn.Month)
            .ForContext("CreatedYear", (short)createdOn.Year)
            .Information(message);
    }
}

public class SeriLogCustomPropertyEnricher : Serilog.Core.ILogEventEnricher
{
    public void Enrich(Serilog.Events.LogEvent logEvent, Serilog.Core.ILogEventPropertyFactory propertyFactory)
    {
        var now = DateTime.Now;

        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("CreatedOn", now));
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("CreatedDay", (byte)now.Day));
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("CreatedMonth", (byte)now.Month));
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("CreatedYear", (Int16)now.Year));
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("MachineName", Environment.MachineName));
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("IPAddress", NetworkUtils.GetServerIpAddress()));
    }
}
