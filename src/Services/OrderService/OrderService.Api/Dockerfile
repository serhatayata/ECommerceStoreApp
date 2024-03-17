#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5007
EXPOSE 6007

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/OrderService/OrderService.Api/OrderService.Api.csproj", "src/Services/OrderService/OrderService.Api/"]
COPY ["src/Shared/Shared/Shared.csproj", "src/Shared/Shared/"]
RUN dotnet restore "./src/Services/OrderService/OrderService.Api/./OrderService.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/OrderService/OrderService.Api"
RUN dotnet build "./OrderService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OrderService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderService.Api.dll"]