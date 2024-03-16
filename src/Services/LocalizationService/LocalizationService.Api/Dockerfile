#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5002
EXPOSE 6002

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/LocalizationService/LocalizationService.Api/LocalizationService.Api.csproj", "src/Services/LocalizationService/LocalizationService.Api/"]
RUN dotnet restore "./src/Services/LocalizationService/LocalizationService.Api/./LocalizationService.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/LocalizationService/LocalizationService.Api"
RUN dotnet build "./LocalizationService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LocalizationService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LocalizationService.Api.dll"]