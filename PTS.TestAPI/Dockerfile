FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["PTS.TestAPI/PTS.TestAPI.csproj", "PTS.TestAPI/"]
RUN dotnet restore "PTS.TestAPI/PTS.TestAPI.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/app/PTS.TestAPI"
RUN dotnet build "PTS.TestAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "PTS.TestAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PTS.TestAPI.dll"]