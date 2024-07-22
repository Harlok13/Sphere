FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/ApiGateway/Harlok.Sphere.ApiGateway.API/Harlok.Sphere.ApiGateway.API.csproj", "src/Services/ApiGateway/Harlok.Sphere.ApiGateway.API/"]
COPY ["src/BuildingBlocks/Harlok.Sphere.Core/Harlok.Sphere.Core.csproj", "src/BuildingBlocks/Harlok.Sphere.Core/"]
COPY ["src/BuildingBlocks/Infrastructure/Harlok.Sphere.Core.Infrastructure.Keycloak/Harlok.Sphere.Core.Infrastructure.Keycloak.csproj", "src/BuildingBlocks/Infrastructure/Harlok.Sphere.Core.Infrastructure.Keycloak/"]
RUN dotnet restore "src/Services/ApiGateway/Harlok.Sphere.ApiGateway.API/Harlok.Sphere.ApiGateway.API.csproj"
COPY . .
WORKDIR "/src/src/Services/ApiGateway/Harlok.Sphere.ApiGateway.API"
RUN dotnet build "Harlok.Sphere.ApiGateway.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Harlok.Sphere.ApiGateway.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Harlok.Sphere.ApiGateway.API.dll"]
