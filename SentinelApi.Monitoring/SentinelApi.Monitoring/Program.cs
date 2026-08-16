using SentinelApi.Monitoring.Extensions;
using SentinelLib.Identity.Security.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSentinelAuthentication(builder.Configuration, requireHttpsMetadata: !builder.Environment.IsDevelopment());

builder.Services.AddSwaggerWithAuthorizationAndDocumentation();

builder.Services.AddControllers();

var app = builder.Build();

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
