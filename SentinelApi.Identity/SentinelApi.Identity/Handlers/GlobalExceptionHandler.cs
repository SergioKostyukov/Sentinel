using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SentinelApi.Identity.Domain.Exceptions;

namespace SentinelApi.Identity.Handlers;

/// <summary>
/// Глобальний обробник помилок для перехоплення та обробки всіх необроблених виключень у сервісі.
/// </summary>
/// <returns>
/// Повертає відповідь з відповідним статусом та деталями помилки у форматі ProblemDetails.
/// </returns>
internal sealed class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct)
    {
        // Скасування операції — окремий випадок, не помилка сервера
        if (exception is OperationCanceledException)
        {
            return HandleCancellation(httpContext);
        }

        var statusCode = exception switch
        {
            AppException appEx => appEx.StatusCode,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        // очікувані бізнес-винятки логуємо як Warn, все інше — як Error
        if (exception is AppException)
            logger.LogWarning(exception, "Handled business exception occurred.");
        else
            logger.LogError(exception, "Unhandled exception occurred.");

        httpContext.Response.StatusCode = statusCode;
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

    private bool HandleCancellation(HttpContext httpContext)
    {
        // Якщо клієнт сам розірвав з'єднання — писати відповідь вже нікуди й немає сенсу
        if (httpContext.RequestAborted.IsCancellationRequested)
        {
            logger.LogWarning("Request was cancelled by the client. Path={Path}", httpContext.Request.Path);
            return true;
        }

        // Скасування з іншої причини (напр. внутрішній таймаут) — 499 Client Closed Request
        logger.LogWarning("Request was cancelled. Path={Path}", httpContext.Request.Path);

        httpContext.Response.StatusCode = 499;
        return true;
    }
}
