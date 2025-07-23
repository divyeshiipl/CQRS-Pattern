namespace Web.Middlewares;

public class AuditLogMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ISeriLogAppService _seriLogAppService;
    private LogDto? auditLogs;

    public AuditLogMiddleware(RequestDelegate next, ISeriLogAppService seriLogAppService)
    {
        _next = next;
        _seriLogAppService = seriLogAppService;
    }

    public async Task Invoke(HttpContext context)
    {
        SelfLog.Enable(msg => System.Diagnostics.Debug.WriteLine(msg));
        var request = await _seriLogAppService.FormatRequestAsync(context.Request);
        var originalBodyStream = context.Response.Body;

        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody;
            await _next(context); // Let the next middleware run
            var responseText = await _seriLogAppService.FormatResponseAsync(responseBody);
            var statusCode = context.Response.StatusCode;

            bool isStatusCodeValid = statusCode is not 500 and not 400 and not 401;
            bool isApiRequest = context.Request?.Path.Value?.Contains("api") == true;
            bool isNotTokenRequest = context.Request?.Path.Value?.Contains("token") != true;
            bool hasRequestOrResponse = !string.IsNullOrWhiteSpace(request) || !string.IsNullOrWhiteSpace(responseText);

            if (isStatusCodeValid && isApiRequest && isNotTokenRequest && hasRequestOrResponse)
            {
                auditLogs ??= new LogDto();
                auditLogs = _seriLogAppService.SetFromContext();
                auditLogs.Request = request;
                auditLogs.Response = $"{statusCode}: {responseText}";

                _seriLogAppService.LogInformation(auditLogs);
            }

            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }
        context.Response.Body = originalBodyStream; // Restore the original body stream
    }
}
