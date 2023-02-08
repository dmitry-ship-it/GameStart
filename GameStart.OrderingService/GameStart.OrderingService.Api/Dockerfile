#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GameStart.OrderingService/GameStart.OrderingService.Api/GameStart.OrderingService.Api.csproj", "GameStart.OrderingService/GameStart.OrderingService.Api/"]
COPY ["GameStart.Shared/GameStart.Shared.csproj", "GameStart.Shared/"]
COPY ["GameStart.OrderingService/GameStart.OrderingService.Application/GameStart.OrderingService.Application.csproj", "GameStart.OrderingService/GameStart.OrderingService.Application/"]
COPY ["GameStart.OrderingService/GameStart.OrderingService.Core/GameStart.OrderingService.Core.csproj", "GameStart.OrderingService/GameStart.OrderingService.Core/"]
COPY ["GameStart.OrderingService/GameStart.OrderingService.Infrastructure/GameStart.OrderingService.Infrastructure.csproj", "GameStart.OrderingService/GameStart.OrderingService.Infrastructure/"]
RUN dotnet restore "GameStart.OrderingService/GameStart.OrderingService.Api/GameStart.OrderingService.Api.csproj"
COPY . .
WORKDIR "/src/GameStart.OrderingService/GameStart.OrderingService.Api"
RUN dotnet build "GameStart.OrderingService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameStart.OrderingService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameStart.OrderingService.Api.dll"]