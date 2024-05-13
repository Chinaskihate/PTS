FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["PTS.TestGeneratorService/PTS.TestGeneratorService.csproj", "PTS.TestGeneratorService/"]
RUN dotnet restore "PTS.TestGeneratorService/PTS.TestGeneratorService.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/app/PTS.TestGeneratorService"
RUN dotnet build "PTS.TestGeneratorService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "PTS.TestGeneratorService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PTS.TestGeneratorService.dll"]