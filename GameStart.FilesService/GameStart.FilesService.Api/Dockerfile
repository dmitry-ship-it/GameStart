#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GameStart.FilesService/GameStart.FilesService.Api/GameStart.FilesService.Api.csproj", "GameStart.FilesService/GameStart.FilesService.Api/"]
COPY ["GameStart.Shared/GameStart.Shared.csproj", "GameStart.Shared/"]
COPY ["GameStart.FilesService/GameStart.FilesService.Common/GameStart.FilesService.Common.csproj", "GameStart.FilesService/GameStart.FilesService.Common/"]
RUN dotnet restore "GameStart.FilesService/GameStart.FilesService.Api/GameStart.FilesService.Api.csproj"
COPY . .
WORKDIR "/src/GameStart.FilesService/GameStart.FilesService.Api"
RUN dotnet build "GameStart.FilesService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameStart.FilesService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameStart.FilesService.Api.dll"]