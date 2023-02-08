#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GameStart.IdentityService/GameStart.IdentityService.Api/GameStart.IdentityService.Api.csproj", "GameStart.IdentityService/GameStart.IdentityService.Api/"]
COPY ["GameStart.IdentityService/GameStart.IdentityService.Common/GameStart.IdentityService.Common.csproj", "GameStart.IdentityService/GameStart.IdentityService.Common/"]
COPY ["GameStart.Shared/GameStart.Shared.csproj", "GameStart.Shared/"]
COPY ["GameStart.IdentityService/GameStart.IdentityService.Data/GameStart.IdentityService.Data.csproj", "GameStart.IdentityService/GameStart.IdentityService.Data/"]
RUN dotnet restore "GameStart.IdentityService/GameStart.IdentityService.Api/GameStart.IdentityService.Api.csproj"
COPY . .
WORKDIR "/src/GameStart.IdentityService/GameStart.IdentityService.Api"
RUN dotnet build "GameStart.IdentityService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameStart.IdentityService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameStart.IdentityService.Api.dll"]