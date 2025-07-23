using Application.Common.Exceptions;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Diagnostics;

namespace Web.Handlers;

public class CustomExceptionHandler : IExceptionHandler
{
    private readonly ILogService _logService;
    private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers;
    private readonly IErrorResponseService _errorResponseService;

    public CustomExceptionHandler(ILogService logService, IErrorResponseService errorResponseServic)
    {
        // Register known exception types and handlers.
        _exceptionHandlers = new()
            {
                { typeof(Application.Common.Exceptions.ValidationException), errorResponseServic.HandleValidationException },
                { typeof(NotFoundException), errorResponseServic.HandleNotFoundException },
                { typeof(UnauthorizedAccessException), errorResponseServic.HandleUnauthorizedAccessException },
                { typeof(ForbiddenAccessException), errorResponseServic.HandleForbiddenAccessException },
                { typeof(CustomErrorException), errorResponseServic.HandleCustomErrorException },
               // { typeof(InvalidOperationException), errorResponseServic.HandleInvalidOperationException }  // Add this

            };
        _errorResponseService = errorResponseServic;
        _logService = logService;
    }


    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logService.LogError(exception);

        var exceptionType = exception.GetType();

        if (_exceptionHandlers.ContainsKey(exceptionType))
        {
            await _exceptionHandlers[exceptionType].Invoke(httpContext, exception);
            return true;
        }

        // If the exception type is not handled, call the generic handler
        await _errorResponseService.HandleUnhandledException(httpContext, exception);
        return true;  // Mark as handled
    }
}