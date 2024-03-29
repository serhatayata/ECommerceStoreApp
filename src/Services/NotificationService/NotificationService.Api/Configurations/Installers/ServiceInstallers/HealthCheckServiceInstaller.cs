namespace NotificationService.Api.Configurations.Installers.ServiceInstallers;

public class HealthCheckServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddHealthChecks()
                .AddRedis(
                    redisConnectionString: configuration["RedisOptions:ConnectionString"],
                    name: "Redis Check",
                    failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy | Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded,
                    tags: new string[] { "redis" })
                .AddElasticsearch(
                    elasticsearchUri: configuration["ElasticSearchOptions:ConnectionString"],
                    name: "ElasticSearch",
                    failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy | Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded,
                    tags: new string[] { "elasticSearch" })
                .AddConsul(
                    c => {
                        var consulAddress = configuration.GetSection("ConsulConfig:Address").Value;
                        var consulUrl = new Uri(consulAddress);

                        c.RequireHttps = false;
                        c.Port = consulUrl.Port;
                        c.HostName = consulUrl.Host;
                    },
                    name: "Consul",
                    failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy | Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded,
                    tags: new string[] { "consul" });

        return Task.CompletedTask;
    }
}
