FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/App/App.API/App.API.csproj", "src/Services/App/App.API/"]
RUN dotnet restore "src/Services/App/App.API/App.API.csproj"
COPY . .
WORKDIR "/src/src/Services/App/App.API"
RUN dotnet build "App.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "App.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "App.API.dll"]
