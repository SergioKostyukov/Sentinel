using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace SentinelApi.Monitoring.Handlers;

/// <summary>
/// Глобальний обробник помилок для перехоплення та обробки всіх необроблених виключень у сервісі.
/// </summary>
/// <returns>
/// Повертає відповідь з відповідним статусом та деталями помилки у форматі ProblemDetails.
/// </returns>
internal sealed class GlobalExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    private readonly Logger _logger = LogManager.GetLogger("GlobalExceptionHandler");

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.Error(exception, "Unhandled exception occurred.");

        httpContext.Response.StatusCode = exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        httpContext.Response.ContentType = "application/problem+json";

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Type = exception.GetType().Name,
                Title = exception.GetType().Name,
                Detail = exception.Message
            }
        });
    }
}
