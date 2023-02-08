#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GameStart.CatalogService/GameStart.CatalogService.Api/GameStart.CatalogService.Api.csproj", "GameStart.CatalogService/GameStart.CatalogService.Api/"]
COPY ["GameStart.CatalogService/GameStart.CatalogService.Common/GameStart.CatalogService.Common.csproj", "GameStart.CatalogService/GameStart.CatalogService.Common/"]
COPY ["GameStart.CatalogService/GameStart.CatalogService.Data/GameStart.CatalogService.Data.csproj", "GameStart.CatalogService/GameStart.CatalogService.Data/"]
COPY ["GameStart.Shared/GameStart.Shared.csproj", "GameStart.Shared/"]
RUN dotnet restore "GameStart.CatalogService/GameStart.CatalogService.Api/GameStart.CatalogService.Api.csproj"
COPY . .
WORKDIR "/src/GameStart.CatalogService/GameStart.CatalogService.Api"
RUN dotnet build "GameStart.CatalogService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameStart.CatalogService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameStart.CatalogService.Api.dll"]