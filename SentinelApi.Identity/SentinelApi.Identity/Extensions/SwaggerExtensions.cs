using System.Reflection;
using Microsoft.OpenApi;

namespace SentinelApi.Identity.Extensions;

internal static class SwaggerExtensions
{
    internal static IServiceCollection AddSwaggerWithAuthorizationAndDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            // Додаємо визначення авторизації через JWT Bearer токен
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Please enter token.",
            });

            // Додаємо вимогу авторизації через JWT Bearer токен
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
