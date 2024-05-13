FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["PTS.AdminAPI/PTS.AdminAPI.csproj", "PTS.AdminAPI/"]
RUN dotnet restore "PTS.AdminAPI/PTS.AdminAPI.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/app/PTS.AdminAPI"
RUN dotnet build "PTS.AdminAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "PTS.AdminAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PTS.AdminAPI.dll"]