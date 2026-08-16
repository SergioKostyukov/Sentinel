using NLog.Web;
using SentinelApi.Identity.Application;
using SentinelApi.Identity.Extensions;
using SentinelApi.Identity.Handlers;
using SentinelApi.Identity.Infrastructure;
using SentinelApi.Identity.Infrastructure.Data;
using SentinelLib.Identity.Security.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddValidation();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddIdentityApplication()
                .AddIdentityInfrastructure(builder.Configuration)
                .AddIdentityInfrastructureData(builder.Configuration);

builder.Services.AddSentinelAuthentication(builder.Configuration, requireHttpsMetadata: !builder.Environment.IsDevelopment());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithAuthorizationAndDocumentation();

var app = builder.Build();

app.UseExceptionHandler();

await app.MigrateDatabaseAsync();
await app.SeedAdminUserAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
