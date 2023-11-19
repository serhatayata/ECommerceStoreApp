﻿using OrderService.Api.Models.Settings;
using System.Web;

namespace OrderService.Api.Configurations.Installers.ServiceInstallers;

public class HealthCheckServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddHealthChecks()
                .AddSqlServer(
                    connectionString: configuration.GetConnectionString("OrderDb"),
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
                    rabbitConnectionString: GetRabbitMqConnectionString(configuration, hostEnvironment.EnvironmentName),
                    name: "RabbitMQ",
                    failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy | Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded,
                    tags: new string[] { "rabbitMQ_MessageBrokerQueue" }
                );
    }

    private static string GetRabbitMqConnectionString(IConfiguration configuration, string env)
    {
        var queueOptions = configuration.GetSection($"QueueSettings:{env}:RabbitMQ").Get<QueueSettings>();

        string rabbitmqconnection = $"amqp://" +
                                    $"{HttpUtility.UrlEncode(queueOptions?.Username)}:" +
                                    $"{HttpUtility.UrlEncode(queueOptions?.Password)}@" +
                                    $"{queueOptions?.Host}:" +
                                    $"{queueOptions?.Port ?? default}/{queueOptions?.VirtualHost}";
        return rabbitmqconnection;
    }
}