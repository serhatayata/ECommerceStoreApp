using Monitoring.BackgroundTasks.Extensions;
using Monitoring.BackgroundTasks.Models.HealthCheckModels;
using Npgsql;
using Quartz;

namespace Monitoring.BackgroundTasks.Services.Concrete;

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
            var gatewayClient = _httpClientFactory.CreateClient("monitoring");

            var requestResult = await gatewayClient.PostGetResponseAsync<List<HealthCheckModel>, string>("api/healthcheck/healthchecks-all", null);

            //Connection
            if (requestResult != null &&
                requestResult.Count > 0)
            {
                var conn = new NpgsqlConnection(connectionString: monitoringConnString);
                conn.Open();

                foreach (var healthCheckItem in requestResult)
                {
                    await using var transaction = await conn.BeginTransactionAsync();

                    if (healthCheckItem.Status == HealthCheckStatusValues.Healthy)
                    {
                        try
                        {
                            string executionCommandText = $"INSERT INTO healthcheck.executions " +
                                                          $"(status, execution_date, uri, service_name) " +
                                                          $"VALUES (@status, @executionDate, @uri, @serviceName) " +
                                                          $"RETURNING id";

                            using var cmd = new NpgsqlCommand(cmdText: executionCommandText,
                                                              connection: conn,
                                                              transaction: transaction);

                            cmd.Parameters.AddWithValue("status", healthCheckItem.Status);
                            cmd.Parameters.AddWithValue("executionDate", DateTime.UtcNow);
                            cmd.Parameters.AddWithValue("uri", healthCheckItem.ServiceUri);
                            cmd.Parameters.AddWithValue("serviceName", healthCheckItem.ServiceName);

                            var executionResult = await cmd.ExecuteScalarAsync();

                            if (executionResult == null)
                            {
                                _logger.LogError($"Health check execution saving error - " +
                                                 $"Saving execution for {healthCheckItem.ServiceName}");
                                continue;
                            }

                            foreach (var info in healthCheckItem.Info)
                            {
                                string infoCommandText = $"INSERT INTO healthcheck.execution_entries " +
                                                         $"(name, status, duration, tags, health_check_execution_id) " +
                                                         $"VALUES (@name, @status, @duration, @tags, @healthCheckExecutionId)";

                                using var infoCmd = new NpgsqlCommand(cmdText: infoCommandText,
                                                                      connection: conn,
                                                                      transaction: transaction);

                                infoCmd.Parameters.AddWithValue("name", info.Key);
                                infoCmd.Parameters.AddWithValue("status", info.Status);
                                infoCmd.Parameters.AddWithValue("duration", info.Duration);
                                infoCmd.Parameters.AddWithValue("tags", string.Join(',', info.Tags));
                                infoCmd.Parameters.AddWithValue("healthCheckExecutionId", executionResult);

                                var executionEntryResult = await infoCmd.ExecuteNonQueryAsync();
                            }

                            await transaction.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Health check execution saving error - Saving execution for {healthCheckItem.ServiceName} - " +
                                             $"Status : {healthCheckItem.Status} - " +
                                             $"Message : {ex.Message}");

                            transaction.Rollback();
                        }
                    }
                    else
                    {
                        try
                        {
                            var failureCommandText = $"INSERT INTO healthcheck.failure " +
                                                     $"(service_name, create_date) " +
                                                     $"VALUES (@serviceName, @createDate)";

                            using var cmd = new NpgsqlCommand(cmdText: failureCommandText,
                                                              connection: conn,
                                                              transaction: transaction);
                            cmd.Connection = conn;

                            cmd.Parameters.AddWithValue("serviceName", healthCheckItem.ServiceName);
                            cmd.Parameters.AddWithValue("createDate", DateTime.UtcNow);

                            var failureResult = await cmd.ExecuteNonQueryAsync();

                            await transaction.CommitAsync();

                            if (failureResult < 1)
                                _logger.LogError($"Health check failure saving error - " +
                                                 $"Saving failure for {healthCheckItem.ServiceName}");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Health check saving error - Saving failure for {healthCheckItem.ServiceName} - " +
                                             $"Status : {healthCheckItem.Status} - " +
                                             $"Message : {ex.Message}");

                            transaction.Rollback();
                        }
                    }
                }
            }

            _logger.LogInformation($"Health check saving finished");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Health check saving exception error - " +
                             $"Message : {ex.Message}");
        }
    }
}
