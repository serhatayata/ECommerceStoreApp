#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5003
EXPOSE 6003

RUN apt-get update && apt-get install -y curl jq 

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/NotificationService/NotificationService.Api/NotificationService.Api.csproj", "src/Services/NotificationService/NotificationService.Api/"]
RUN dotnet restore "./src/Services/NotificationService/NotificationService.Api/./NotificationService.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/NotificationService/NotificationService.Api"
RUN dotnet build "./NotificationService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./NotificationService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

RUN apt-get update && apt-get install -y curl

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificationService.Api.dll"]