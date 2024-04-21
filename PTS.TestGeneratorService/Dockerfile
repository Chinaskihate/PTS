FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PTS.TestGeneratorService/PTS.TestGeneratorService.csproj", "PTS.TestGeneratorService/"]
COPY ["PTS.Contracts/PTS.Contracts.csproj", "PTS.Contracts/"]
COPY ["PTS.Backend/PTS.Backend.csproj", "PTS.Backend/"]
COPY ["PTS.Persistence/PTS.Persistence.csproj", "PTS.Persistence/"]
RUN dotnet restore "PTS.TestGeneratorService/PTS.TestGeneratorService.csproj"
COPY . .
WORKDIR "/src/PTS.TestGeneratorService"
RUN dotnet build "PTS.TestGeneratorService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PTS.TestGeneratorService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PTS.TestGeneratorService.dll"]
