FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MsStore.Users.Api/MsStore.Users.Api.csproj", "MsStore.Users.Api/"]
RUN dotnet restore "MsStore.Users.Api/MsStore.Users.Api.csproj"
COPY . .
WORKDIR "/src/MsStore.Users.Api"
RUN dotnet build "./MsStore.Users.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MsStore.Users.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MsStore.Users.Api.dll"]
