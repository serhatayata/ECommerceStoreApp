#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app
EXPOSE 4000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/BuildingBlocks/StateMachine/SagaStateMachineWorkerService/SagaStateMachineWorkerService.csproj", "src/BuildingBlocks/StateMachine/SagaStateMachineWorkerService/"]
COPY ["src/Shared/Shared/Shared.csproj", "src/Shared/Shared/"]
RUN dotnet restore "./src/BuildingBlocks/StateMachine/SagaStateMachineWorkerService/./SagaStateMachineWorkerService.csproj"
COPY . .
WORKDIR "/src/src/BuildingBlocks/StateMachine/SagaStateMachineWorkerService"
RUN dotnet build "./SagaStateMachineWorkerService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SagaStateMachineWorkerService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SagaStateMachineWorkerService.dll"]