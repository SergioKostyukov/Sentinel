FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Directory.Build.props Directory.Packages.props ./
COPY SentinelApi.Monitoring/SentinelApi.Monitoring.csproj SentinelApi.Monitoring/
COPY SentinelApi.Monitoring.Domain/SentinelApi.Monitoring.Domain.csproj SentinelApi.Monitoring.Domain/
COPY SentinelApi.Monitoring.Application/SentinelApi.Monitoring.Application.csproj SentinelApi.Monitoring.Application/
COPY SentinelApi.Monitoring.Infrastructure/SentinelApi.Monitoring.Infrastructure.csproj SentinelApi.Monitoring.Infrastructure/
COPY SentinelApi.Monitoring.Infrastructure.Data/SentinelApi.Monitoring.Infrastructure.Data.csproj SentinelApi.Monitoring.Infrastructure.Data/
RUN dotnet restore SentinelApi.Monitoring/SentinelApi.Monitoring.csproj

COPY . .
RUN dotnet publish SentinelApi.Monitoring/SentinelApi.Monitoring.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "SentinelApi.Monitoring.dll"]
