FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Install Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_14.x | bash - \
    && apt-get install -y \
        nodejs \
    && rm -rf /var/lib/apt/lists/* \
    
WORKDIR /src
COPY ["App.API.csproj", "App.API/"]
COPY ["Sphere/App.Infra/App.Infra.csproj", "App.Infra/"]
COPY ["App.Application/App.Application.csproj", "App.Application"]
COPY ["App.SignalR/App.SignalR.csproj", "App.SignalR"]
COPY ["App.Contracts/App.Contracts.csproj", "App.Contracts"]
COPY ["App.Domain/App.Domain.csproj", "App.Domain"]
RUN dotnet restore "App.API/App.API.csproj"
COPY . .
WORKDIR "/src/Sphere"
RUN ls -a /src/Sphere
RUN dotnet build "App.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "App.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "App.API.dll"]
