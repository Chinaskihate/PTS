FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["PTS.AuthAPI/PTS.AuthAPI.csproj", "PTS.AuthAPI/"]
RUN dotnet restore "PTS.AuthAPI/PTS.AuthAPI.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/app/PTS.AuthAPI"
RUN dotnet build "PTS.AuthAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "PTS.AuthAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PTS.AuthAPI.dll"]