
using CampaignService.Api.Attributes;

namespace CampaignService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 6)]
public class HealthCheckServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddHealthChecks()
        .AddSqlServer(
            connectionString: configuration.GetConnectionString("CampaignDb") ?? string.Empty,
            healthQuery: "SELECT 1",
            name: "MSSQL Server check",
            failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy | Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded,
            tags: new string[] { "db", "mssql", "sqlserver" }
        )
        .AddRedis(
            redisConnectionString: configuration["RedisOptions:ConnectionString"] ?? string.Empty,
            name: "Redis Check",
            failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy | Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded,
            tags: new string[] { "redis" }
        )
        .AddElasticsearch(
            elasticsearchUri: configuration["ElasticSearchOptions:ConnectionString"] ?? string.Empty,
            name: "ElasticSearch",
            failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy | Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded,
            tags: new string[] { "elasticSearch" }
        )
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
    }
}
