using Monitoring.BackgroundTasks.Extensions;
using Monitoring.BackgroundTasks.Models.HealthCheckModels;
using Npgsql;
using Quartz;
using System.Drawing;

namespace Monitoring.BackgroundTasks.Services;

public class HealthCheckSaveJob : IJob
{
    private readonly ILogger<HealthCheckSaveJob> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    
    private readonly string monitoringConnString;

    public HealthCheckSaveJob(
        IHttpClientFactory httpClientFactory,
        ILogger<HealthCheckSaveJob> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;

        monitoringConnString = _configuration.GetConnectionString("MonitoringDb");
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            _logger.LogInformation("Health check saving started");
            var gatewayClient = _httpClientFactory.CreateClient("gateway");

            var requestResult = await gatewayClient.PostGetResponseAsync<List<HealthCheckModel>, string>("monitoring/api/healthcheck/healthchecks-all", null);

            //Connection
            if (requestResult != null && 
                requestResult.Count > 0)
            {
                var conn = new NpgsqlConnection(connectionString: monitoringConnString);
                conn.Open();

                using var cmd = new NpgsqlCommand();
                cmd.Connection = conn;

                foreach (var healthCheckItem in requestResult)
                {
                    if (healthCheckItem.Status == HealthCheckStatus.Healthy)
                    {
                        cmd.CommandText = $"INSERT INTO healthcheck.executions " +
                                          $"(status, execution_date, uri, service_name) " +
                                          $"VALUES (@status, @executionDate, @uri, @serviceName)";

                        cmd.Parameters.AddWithValue("status", healthCheckItem.Status);
                        cmd.Parameters.AddWithValue("executionDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("uri", healthCheckItem.ServiceUri);
                        cmd.Parameters.AddWithValue("serviceName", healthCheckItem.ServiceName);

                        var executionResult = await cmd.ExecuteNonQueryAsync();

                        if (executionResult < 1)
                            _logger.LogError($"Health check saving error - Saving execution for {healthCheckItem.ServiceName}");

                        foreach (var info in healthCheckItem.Info)
                        {
                            cmd.CommandText = $"INSERT INTO healthcheck.execution_entries " +
                                              $"(name, status, duration, tags) " +
                                              $"VALUES (@name, @status, @duration, @tags)";

                            cmd.Parameters.AddWithValue("name", info.Key);
                            cmd.Parameters.AddWithValue("status", info.Status);
                            cmd.Parameters.AddWithValue("duration", info.Duration);
                            cmd.Parameters.AddWithValue("tags", string.Join(',', info.Tags));

                            var executionEntryResult = await cmd.ExecuteNonQueryAsync();

                            if (executionEntryResult < 1)
                                _logger.LogError($"Health check saving error - Saving execution entry for {healthCheckItem.ServiceName} - {info.Key}");
                        }
                    }
                    else
                    {
                        cmd.CommandText = $"INSERT INTO healthcheck.failure " +
                                          $"(service_name, create_date) " +
                                          $"VALUES (@serviceName, @createDate)";

                        cmd.Parameters.AddWithValue("serviceName", healthCheckItem.ServiceName);
                        cmd.Parameters.AddWithValue("createDate", DateTime.Now);

                        var failureResult = await cmd.ExecuteNonQueryAsync();

                        if (failureResult < 1)
                            _logger.LogError($"Health check saving error - Saving failure for {healthCheckItem.ServiceName}");
                    }
                }
            }

            _logger.LogInformation($"Health check saving finished");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Health check saving exception error - Message : {ex.Message}");
        }
    }
}
