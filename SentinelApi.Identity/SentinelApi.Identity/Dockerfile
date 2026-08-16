FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY SentinelApi.Identity/Directory.Build.props SentinelApi.Identity/Directory.Packages.props SentinelApi.Identity/
COPY SentinelApi.Identity/SentinelApi.Identity/SentinelApi.Identity.csproj SentinelApi.Identity/SentinelApi.Identity/
COPY SentinelApi.Identity/SentinelApi.Identity.Domain/SentinelApi.Identity.Domain.csproj SentinelApi.Identity/SentinelApi.Identity.Domain/
COPY SentinelApi.Identity/SentinelApi.Identity.Application/SentinelApi.Identity.Application.csproj SentinelApi.Identity/SentinelApi.Identity.Application/
COPY SentinelApi.Identity/SentinelApi.Identity.Infrastructure/SentinelApi.Identity.Infrastructure.csproj SentinelApi.Identity/SentinelApi.Identity.Infrastructure/
COPY SentinelApi.Identity/SentinelApi.Identity.Infrastructure.Data/SentinelApi.Identity.Infrastructure.Data.csproj SentinelApi.Identity/SentinelApi.Identity.Infrastructure.Data/
COPY SentinelLib.Identity.Security/SentinelLib.Identity.Security/SentinelLib.Identity.Security.csproj SentinelLib.Identity.Security/SentinelLib.Identity.Security/
RUN dotnet restore SentinelApi.Identity/SentinelApi.Identity/SentinelApi.Identity.csproj

COPY SentinelApi.Identity/ SentinelApi.Identity/
COPY SentinelLib.Identity.Security/SentinelLib.Identity.Security/ SentinelLib.Identity.Security/SentinelLib.Identity.Security/
RUN dotnet publish SentinelApi.Identity/SentinelApi.Identity/SentinelApi.Identity.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "SentinelApi.Identity.dll"]
