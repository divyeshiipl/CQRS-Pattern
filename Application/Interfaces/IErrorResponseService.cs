namespace Application.Interfaces;

public interface IErrorResponseService
{
    Task HandleUnhandledException(HttpContext httpContext, Exception ex);


    Task HandleValidationException(HttpContext httpContext, Exception ex);


    Task HandleNotFoundException(HttpContext httpContext, Exception ex);


    Task HandleUnauthorizedAccessException(HttpContext httpContext, Exception ex);


    Task HandleForbiddenAccessException(HttpContext httpContext, Exception ex);

    Task HandleCustomErrorException(HttpContext httpContext, Exception ex);



}