using SentinelApi.Monitoring.Application;
using SentinelApi.Monitoring.Extensions;
using SentinelApi.Monitoring.Handlers;
using SentinelApi.Monitoring.Infrastructure;
using SentinelApi.Monitoring.Infrastructure.Data;
using SentinelLib.Identity.Security.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddServices(builder.Configuration)
                .AddInfrastructure(builder.Configuration)
                .AddInfrastructureData(builder.Configuration);

builder.Services.AddSentinelAuthentication(builder.Configuration, requireHttpsMetadata: !builder.Environment.IsDevelopment());

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddSwaggerWithAuthorizationAndDocumentation();

var app = builder.Build();

app.UseExceptionHandler();

await app.MigrateDatabaseAsync();

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
