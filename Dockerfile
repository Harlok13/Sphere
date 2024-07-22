FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Install Node.js
#RUN curl -fsSL https://deb.nodesource.com/setup_14.x | bash - \
#    && apt-get install -y \
#        nodejs \
#    && rm -rf /var/lib/apt/lists/*

WORKDIR /src
COPY ["src/Services/App/App.API/*.csproj", "App.API/"]
COPY ["src/Services/App/App.Application/*.csproj", "App.Application/"]
COPY ["src/BuildingBlocks/Infrastructure/*.csproj", "Infrastructure/"]
COPY ["src/Services/App/App.Domain/*.csproj", "App.Domain/"]
COPY ["src/Services/App/App.Contracts/*.csproj", "App.Contracts/"]
COPY ["src/Services/App/App.SignalR/*.csproj", "App.SignalR/"]
RUN dotnet restore "App.API/App.API.csproj"
COPY . .
WORKDIR "/src"
RUN #ls -a "/src/src/Services/App"
RUN dotnet build "src/Services/App/App.API/App.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/Services/App/App.API/App.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "App.API.dll"]
