#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 50010

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/MonitoringService/Monitoring.BackgroundTasks/Monitoring.BackgroundTasks.csproj", "src/Services/MonitoringService/Monitoring.BackgroundTasks/"]
RUN dotnet restore "./src/Services/MonitoringService/Monitoring.BackgroundTasks/./Monitoring.BackgroundTasks.csproj"
COPY . .
WORKDIR "/src/src/Services/MonitoringService/Monitoring.BackgroundTasks"
RUN dotnet build "./Monitoring.BackgroundTasks.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Monitoring.BackgroundTasks.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Monitoring.BackgroundTasks.dll"]