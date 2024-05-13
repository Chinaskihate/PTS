FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["PTS.StatisticsAPI/PTS.StatisticsAPI.csproj", "PTS.StatisticsAPI/"]
RUN dotnet restore "PTS.StatisticsAPI/PTS.StatisticsAPI.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/app/PTS.StatisticsAPI"
RUN dotnet build "PTS.StatisticsAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "PTS.StatisticsAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PTS.StatisticsAPI.dll"]