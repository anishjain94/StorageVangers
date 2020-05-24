FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["StorageVangers.Api/StorageVangers.Api.csproj", "StorageVangers.Api/"]
RUN dotnet restore "StorageVangers.Api/StorageVangers.Api.csproj"
COPY . .
WORKDIR /src/StorageVangers.Api
RUN dotnet build "StorageVangers.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "StorageVangers.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "StorageVangers.Api.dll"]
