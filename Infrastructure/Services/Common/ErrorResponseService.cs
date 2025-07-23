namespace Infrastructure.Services.Common;

public class ErrorResponseService : IErrorResponseService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ReturnResult<string> _returnResult;
    private readonly ISeriLogAppService _seriLogAppService;
    private LogDto? errorLogs;

    public ErrorResponseService(
        IHttpContextAccessor httpContextAccessor,
        ISeriLogAppService seriLogAppService)
    {
        _httpContextAccessor = httpContextAccessor;
        _seriLogAppService = seriLogAppService;
        _returnResult = new ReturnResult<string>();
    }

    public async Task HandleUnhandledException(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/json";
        _returnResult.Errors.Clear();
        _returnResult.Errors.Add(new Item(CommonRepositoryMessages.InternalServerErrorTitle
            , CommonRepositoryMessages.InternalServerErrorDetails));

#if DEBUG
        _returnResult.Errors.Add(new Item("Exception", ex.Message));
#endif

        await httpContext.Response.WriteAsJsonAsync(_returnResult);
    }

    public async Task HandleValidationException(HttpContext httpContext, Exception ex)
    {
        var exception = (ValidationException)ex;
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        _returnResult.Errors.Clear();
        _returnResult.Errors.AddRange(exception.ErrorList);
        var responseJson = JsonSerializer.Serialize(_returnResult);
        await httpContext.Response.WriteAsJsonAsync(_returnResult);
        if (errorLogs == null)
        {
            errorLogs = new LogDto();
        }
        errorLogs = _seriLogAppService.SetFromContext();
        errorLogs.Request = await _seriLogAppService.FormatRequestAsync(httpContext.Request);
        errorLogs.Response = responseJson;
        errorLogs.StatusCode = httpContext.Response.StatusCode;
        _seriLogAppService.LogError(errorLogs, exception);
    }

    public async Task HandleNotFoundException(HttpContext httpContext, Exception ex)
    {
        var exception = (NotFoundException)ex;
        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
        _returnResult.Errors.Clear();
        _returnResult.Errors.Add(new Item("Not Found", ex.Message));
        await httpContext.Response.WriteAsJsonAsync(_returnResult);
    }

    public async Task HandleUnauthorizedAccessException(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
        _returnResult.Errors.Clear();
        _returnResult.Errors.Add(new Item(CommonRepositoryMessages.UnauthorizedAccessTitle
            , CommonRepositoryMessages.UnauthorizedAccessDetails));
        await httpContext.Response.WriteAsJsonAsync(_returnResult);
    }

    public async Task HandleForbiddenAccessException(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
        _returnResult.Errors.Clear();
        _returnResult.Errors.Add(new Item(CommonRepositoryMessages.ForbiddenAccessTitle
            , CommonRepositoryMessages.ForbiddenAccessDetails));
        await httpContext.Response.WriteAsJsonAsync(_returnResult);
    }

    public async Task HandleCustomErrorException(HttpContext httpContext, Exception ex)
    {
        var exception = (CustomErrorException)ex;

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        _returnResult.Errors.Clear();
        _returnResult.Errors.Add(new Item(exception.Name, exception.CustomErrorMessage));

        await httpContext.Response.WriteAsJsonAsync(_returnResult);
    }
}
