FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.1-alpine AS base
WORKDIR /app
EXPOSE 6000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.100-alpine AS build
WORKDIR /src
COPY WebScraping.Core/WebScraping.Core.csproj WebScraping.Core/
COPY WebScraping.Web/WebScraping.Web.csproj WebScraping.Web/
COPY WebScraping.Infrastructure/WebScraping.Infrastructure.csproj WebScraping.Infrastructure/
RUN dotnet restore WebScraping.Web/WebScraping.Web.csproj

COPY . .
WORKDIR /src
RUN dotnet build WebScraping.Web/WebScraping.Web.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish WebScraping.Web/WebScraping.Web.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebScraping.dll"]