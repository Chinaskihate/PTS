FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PTS.AuthAPI/PTS.AuthAPI.csproj", "PTS.AuthAPI/"]
RUN dotnet restore "./PTS.AuthAPI/./PTS.AuthAPI.csproj"
COPY . .
WORKDIR "/src/PTS.AuthAPI"
RUN dotnet build "./PTS.AuthAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PTS.AuthAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PTS.AuthAPI.dll"]