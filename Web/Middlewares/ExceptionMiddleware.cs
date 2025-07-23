namespace Web.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ISeriLogAppService _seriLogAppService;
    private LogDto? errorLogs;

    public ExceptionMiddleware(RequestDelegate next, ISeriLogAppService seriLogAppService)
    {
        _next = next;
        _seriLogAppService = seriLogAppService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            if (context.Response.StatusCode == 500)
            {
                var exception = new Exception(responseText);
                await HandleExceptionAsync(context, exception);
            }
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        string errors = string.Empty;
        context.Response.ContentType = "application/json";

        if (exception.Source == "FluentValidation")
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            ValidationException validationException = (ValidationException)exception;
            List<ValidationFailure> validationFailures = (List<ValidationFailure>)validationException.Errors;
            errors = string.Join(", ", validationFailures.Select(failure => failure.ErrorMessage));
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            errors = exception.Message;
        }
        string request = await _seriLogAppService.FormatRequestAsync(context.Request);
        string response = errors;
        if (errorLogs == null)
        {
            errorLogs = new LogDto();
        }
        errorLogs = _seriLogAppService.SetFromContext();
        errorLogs.Request = request;
        errorLogs.Response = response;
        errorLogs.StatusCode = context.Response.StatusCode;
        _seriLogAppService.LogError(errorLogs, exception);
    }
}
