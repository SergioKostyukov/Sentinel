using System.Reflection;
using Microsoft.OpenApi;

namespace SentinelApi.Monitoring.Extensions;

internal static class SwaggerExtensions
{
    internal static IServiceCollection AddSwaggerWithAuthorizationAndDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            // Add security definition for JWT Bearer token
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Please enter token.",
            });

            // Add security requirement for JWT Bearer token
            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            });

            // Завантажуємо XML-документацію
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });

        return services;
    }
}
