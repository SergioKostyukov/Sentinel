FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY SentinelApi.Monitoring/Directory.Build.props SentinelApi.Monitoring/Directory.Packages.props SentinelApi.Monitoring/
COPY SentinelApi.Monitoring/SentinelApi.Monitoring/SentinelApi.Monitoring.csproj SentinelApi.Monitoring/SentinelApi.Monitoring/
COPY SentinelApi.Monitoring/SentinelApi.Monitoring.Domain/SentinelApi.Monitoring.Domain.csproj SentinelApi.Monitoring/SentinelApi.Monitoring.Domain/
COPY SentinelApi.Monitoring/SentinelApi.Monitoring.Application/SentinelApi.Monitoring.Application.csproj SentinelApi.Monitoring/SentinelApi.Monitoring.Application/
COPY SentinelApi.Monitoring/SentinelApi.Monitoring.Infrastructure/SentinelApi.Monitoring.Infrastructure.csproj SentinelApi.Monitoring/SentinelApi.Monitoring.Infrastructure/
COPY SentinelApi.Monitoring/SentinelApi.Monitoring.Infrastructure.Data/SentinelApi.Monitoring.Infrastructure.Data.csproj SentinelApi.Monitoring/SentinelApi.Monitoring.Infrastructure.Data/
COPY SentinelLib.Identity.Security/SentinelLib.Identity.Security/SentinelLib.Identity.Security.csproj SentinelLib.Identity.Security/SentinelLib.Identity.Security/
COPY SentinelLib.Monitoring.SDK/SentinelLib.Monitoring.SDK/SentinelLib.Monitoring.SDK.csproj SentinelLib.Monitoring.SDK/SentinelLib.Monitoring.SDK/
RUN dotnet restore SentinelApi.Monitoring/SentinelApi.Monitoring/SentinelApi.Monitoring.csproj

COPY SentinelApi.Monitoring/ SentinelApi.Monitoring/
COPY SentinelLib.Identity.Security/SentinelLib.Identity.Security/ SentinelLib.Identity.Security/SentinelLib.Identity.Security/
COPY SentinelLib.Monitoring.SDK/SentinelLib.Monitoring.SDK/ SentinelLib.Monitoring.SDK/SentinelLib.Monitoring.SDK/
RUN dotnet publish SentinelApi.Monitoring/SentinelApi.Monitoring/SentinelApi.Monitoring.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "SentinelApi.Monitoring.dll"]
