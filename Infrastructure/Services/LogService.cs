namespace Infrastructure.Services;

public class LogService : ILogService
{
    private readonly ILogger<LogService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LogService(ILogger<LogService> logger, IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }
    public void LogError(Exception ex)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        //var applicationId = _configuration.GetValue<int>("AppSettings:ApplicationId");
        //DateTime createdOn = DateTime.Now;
        // var context = _httpContextAccessor.HttpContext;
        // var correlationId = Convert.ToString(httpContext?.Items["CorrelationId"]);
        var requestId = Convert.ToString(httpContext?.Items["RequestId"]);

        //  _logger.LogError(ex, "An error occurred. RequestId: {RequestId}, ApplicationId: {ApplicationId}, CreatedOn: {CreatedOn}", requestId, applicationId, createdOn);
        _logger.LogError(ex, "RequestId: {RequestId} ErrorMessage: {ErrorMessage}", requestId, ex.Message);


    }

    public void LogAudit(string requestBody,
        string responseBodyText, string queryParameters, DateTime createdOn, DateTime updatedOn)
    {
        var context = _httpContextAccessor.HttpContext;

        if (context == null)
        {
            return;
        }

        var applicationId = _configuration.GetValue<int>("AppSettings:ApplicationId");
        var correlationId = context.Items["CorrelationId"];
        var requestId = context.Items["RequestId"];
        string ipAddress = context.Connection?.RemoteIpAddress?.ToString() ?? string.Empty;

        _logger.LogInformation("RequestId: {RequestId} CorrelationId: {CorrelationId} RequestURL:{RequestURL} IPAddress:{IPAddress} Action:{Action} Request:{Request} Response:{Response} QueryParameters: {QueryParameters} StatusCode:{StatusCode} HttpVerb:{HttpVerb} CreatedOn:{CreatedOn} CreatedDay:{CreatedDay}  CreatedMonth:{CreatedMonth} CreatedYear:{CreatedYear} MachineName:{MachineName} ApplicationId:{ApplicationId} UpdatedOn:{UpdatedOn}",
        requestId, correlationId, CommonUrlHelper.GetAbsoluteUrl(context.Request), ipAddress, context.Request.Path, requestBody, responseBodyText, queryParameters, context.Response.StatusCode, context.Request.Method, createdOn, (byte)createdOn.Date.Day, (byte)createdOn.Date.Month, (short)createdOn.Date.Year, Environment.MachineName.ToString(), applicationId, updatedOn);

    }

    public void LogInfo(string message)
    {
        var context = _httpContextAccessor.HttpContext;

        if (context == null)
        {
            return;
        }

        var applicationId = _configuration.GetValue<int>("AppSettings:ApplicationId");
        var correlationId = context.Items["CorrelationId"];
        var requestId = context.Items["RequestId"];

        _logger.LogInformation("RequestId: {RequestId}, CorrelationId: {CorrelationId}, ApplicationId:{ApplicationId},  Message:{Message} ",
        requestId, correlationId, applicationId, message);
    }

}
