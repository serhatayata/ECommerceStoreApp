﻿using PaymentService.Api.Models.Settings;
using System.Web;

namespace PaymentService.Api.Configurations.Installers.ServiceInstallers;

public class HealthCheckServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddHealthChecks()
                .AddSqlServer(
                    connectionString: configuration.GetConnectionString("PaymentDb"),
                    healthQuery: "SELECT 1",
                    name: "MSSQL Server check",
                    failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy | Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded,
                    tags: new string[] { "db", "mssql", "sqlserver" }
                )
                .AddRedis(
                    redisConnectionString: configuration["RedisOptions:ConnectionString"],
                    name: "Redis Check",
                    failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy | Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded,
                    tags: new string[] { "redis" }
                )
                .AddElasticsearch(
                    elasticsearchUri: configuration["ElasticSearchOptions:ConnectionString"],
                    name: "ElasticSearch",
                    failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy | Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded,
                    tags: new string[] { "elasticSearch" }
                )
                .AddRabbitMQ(
                    rabbitConnectionString: configuration.GetConnectionString("RabbitMQ"),
                    name: "RabbitMQ",
                    failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy | Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded,
                    tags: new string[] { "rabbitMQ_MessageBrokerQueue" }
                );

        return Task.CompletedTask;
    }
}
