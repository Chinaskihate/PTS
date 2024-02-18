FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dotnet
WORKDIR /src
COPY "./PTS.IdentityServer/PTS.IdentityServer.csproj" "./PTS.IdentityServer/PTS.IdentityServer.csproj"
RUN dotnet restore "./PTS.IdentityServer/PTS.IdentityServer.csproj"
COPY "./PTS.IdentityServer" "./PTS.IdentityServer"
RUN dotnet publish "./PTS.IdentityServer/PTS.IdentityServer.csproj" -c Release -o /app/publish

FROM node:lts AS node
WORKDIR /src
COPY "./PTS.Ui/package.json" "./PTS.Ui/package.json"
COPY "./PTS.Ui/package-lock.json" "./PTS.Ui/package-lock.json"
RUN cd PTS.Ui && npm install
COPY "./PTS.Ui" "./PTS.Ui"
RUN cd PTS.Ui && npm run build

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=dotnet "/app/publish" "."
COPY --from=node "/src/PTS.Ui/build" "./wwwroot"
ENTRYPOINT ["dotnet", "PTS.IdentityServer.dll"]
