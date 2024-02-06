FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PTS.StatisticsService/PTS.StatisticsService.csproj", "PTS.StatisticsService/"]
RUN dotnet restore "PTS.StatisticsService/PTS.StatisticsService.csproj"
COPY . .
WORKDIR "/src/PTS.StatisticsService"
RUN dotnet build "PTS.StatisticsService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PTS.StatisticsService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PTS.StatisticsService.dll"]
