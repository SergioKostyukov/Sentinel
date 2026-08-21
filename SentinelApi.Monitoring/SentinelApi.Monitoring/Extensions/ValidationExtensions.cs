using FluentValidation;
using FluentValidation.AspNetCore;

namespace SentinelApi.Monitoring.Extensions;

/// <summary>
/// Конфігурація FluentValidation для валідації моделей запитів.
/// </summary>
internal static class ValidationExtensions
{
    internal static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);

        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();

        return services;
    }
}
