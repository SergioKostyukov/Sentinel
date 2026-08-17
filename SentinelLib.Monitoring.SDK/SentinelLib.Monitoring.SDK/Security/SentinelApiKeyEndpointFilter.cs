using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SentinelLib.Monitoring.SDK.Security;

/// <summary>
/// Endpoint filter, що перевіряє наявність та коректність API-ключа
/// у заголовку <see cref="SentinelHeaders.ApiKey"/> перед виконанням handler'а.
/// </summary>
/// <remarks>
/// Повертає:
/// <list type="bullet">
/// <item><description>500, якщо <see cref="SentinelMonitoringOptions.ApiKey"/> не налаштовано;</description></item>
/// <item><description>401, якщо заголовок відсутній або ключ не збігається.</description></item>
/// </list>
/// Застосовується через <c>RequireSentinelApiKey()</c>.
/// </remarks>
public sealed class SentinelApiKeyEndpointFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var options = context.HttpContext.RequestServices
            .GetRequiredService<IOptions<SentinelMonitoringOptions>>().Value;

        if (string.IsNullOrEmpty(options.ApiKey))
        {
            return Results.Problem("Sentinel API key is not configured.", statusCode: StatusCodes.Status500InternalServerError);
        }

        if (!context.HttpContext.Request.Headers.TryGetValue(SentinelHeaders.ApiKey, out var provided) ||
            !FixedTimeEquals(provided.ToString(), options.ApiKey))
        {
            return Results.Unauthorized();
        }

        return await next(context);
    }

    // Порівняння без "timing attack" — важливо для ключів/секретів
    private static bool FixedTimeEquals(string a, string b)
    {
        var bytesA = Encoding.UTF8.GetBytes(a);
        var bytesB = Encoding.UTF8.GetBytes(b);

        if (bytesA.Length != bytesB.Length)
        {
            // Порівнюємо все одно, щоб не "зливати" довжину через таймінг
            _ = CryptographicOperations.FixedTimeEquals(bytesA, bytesA);
            return false;
        }

        return CryptographicOperations.FixedTimeEquals(bytesA, bytesB);
    }
}
