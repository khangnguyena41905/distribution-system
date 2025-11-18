FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SRC/GATEWAY/GATEWAY.API/GATEWAY.API.csproj", "SRC/GATEWAY/GATEWAY.API/"]
COPY ["SRC/CONTRACT/CONTRACT.csproj", "SRC/CONTRACT/"]
RUN dotnet restore "SRC/GATEWAY/GATEWAY.API/GATEWAY.API.csproj"
COPY . .
WORKDIR "/src/SRC/GATEWAY/GATEWAY.API"
RUN dotnet build "GATEWAY.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "GATEWAY.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GATEWAY.API.dll"]
