FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/UserInteraction/UserInteraction.API/UserInteraction.API.csproj", "src/Services/UserInteraction/UserInteraction.API/"]
RUN dotnet restore "src/Services/UserInteraction/UserInteraction.API/UserInteraction.API.csproj"
COPY . .
WORKDIR "/src/src/Services/UserInteraction/UserInteraction.API"
RUN dotnet build "UserInteraction.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserInteraction.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserInteraction.API.dll"]
