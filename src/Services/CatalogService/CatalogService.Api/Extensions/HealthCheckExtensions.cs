using CatalogService.Api.Models.Settings;
using HealthChecks.Consul;
using System.Reflection;
using System.Web;

namespace CatalogService.Api.Extensions;

public static class HealthCheckExtensions
{
    public static void AddHealthCheckServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
                .AddSqlServer(
                    connectionString: configuration.GetConnectionString("DefaultConnection"),
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
                .AddMongoDb(
                    mongodbConnectionString: configuration["MongoDB:ConnectionURI"],
                    mongoDatabaseName: configuration["MongoDB:DatabaseName"],
                    name: "MongoDB",
                    failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy | Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded,
                    tags: new string[] { "mongoDB" }
                )
                .AddElasticsearch(
                    elasticsearchUri: configuration["ElasticSearchOptions:ConnectionString"],
                    name: "ElasticSearch",
                    failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy | Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded,
                    tags: new string[] { "elasticSearch" }
                )
                .AddRabbitMQ(
                    rabbitConnectionString: GetRabbitMqConnectionString(configuration),
                    name: "RabbitMQ",
                    failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy | Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded,
                    tags: new string[] { "rabbitMQ_MessageBrokerQueue" }
                );

        services.AddGrpcHealthChecks()
                .AddCheck(
                    name: $"{Assembly.GetCallingAssembly().GetName().Name}-HealthCheck",
                    check: () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy(),
                    tags: new string[] { "Grpc" }
                );
    }

    private static string GetRabbitMqConnectionString(IConfiguration configuration)
    {
        var queueOptions = configuration.GetSection("MessageBroker:QueueSettings").Get<MessageBrokerQueueSettings>();

        string rabbitmqconnection = $"amqp://" +
                                    $"{HttpUtility.UrlEncode(queueOptions?.UserName)}:" +
                                    $"{HttpUtility.UrlEncode(queueOptions?.Password)}@" +
                                    $"{queueOptions?.HostName}:" +
                                    $"{queueOptions?.Port ?? default}/{queueOptions?.VirtualHost}";
        return rabbitmqconnection;
    }
}