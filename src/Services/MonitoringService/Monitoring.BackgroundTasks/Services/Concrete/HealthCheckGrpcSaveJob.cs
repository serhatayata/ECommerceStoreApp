using Monitoring.BackgroundTasks.Extensions;
using Monitoring.BackgroundTasks.Models.HealthCheckModels;
using Monitoring.BackgroundTasks.Utilities.Results;
using Npgsql;
using Quartz;

namespace Monitoring.BackgroundTasks.Services.Concrete;

public class HealthCheckGrpcSaveJob : IJob
{
    private readonly ILogger<HealthCheckGrpcSaveJob> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    private readonly string monitoringConnString;

    public HealthCheckGrpcSaveJob(
        ILogger<HealthCheckGrpcSaveJob> logger, 
        IHttpClientFactory httpClientFactory, 
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

            var request = await gatewayClient.PostGetResponseAsync<DataResult<List<HealthCheckGrpcModel>>, string>("api/healthcheck/get-all-grpc", null);
            var requestResult = request?.Data ?? new List<HealthCheckGrpcModel>();

            if (requestResult.Count > 0)
            {
                var conn = new NpgsqlConnection(connectionString: monitoringConnString);
                conn.Open();

                foreach (var healthCheckItem in requestResult)
                {
                    await using var transaction = await conn.BeginTransactionAsync();

                    if (healthCheckItem.Status == Models.Enums.HealthCheckGrpcStatus.SERVING)
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

                            cmd.Parameters.AddWithValue("status", healthCheckItem.Status.ToString());
                            cmd.Parameters.AddWithValue("executionDate", DateTime.UtcNow);
                            cmd.Parameters.AddWithValue("uri", healthCheckItem.ServiceUri);
                            cmd.Parameters.AddWithValue("serviceName", healthCheckItem.ServiceName);

                            var executionResult = await cmd.ExecuteScalarAsync();

                            if (executionResult == null)
                            {
                                _logger.LogError($"Health check GRPC execution saving error - " +
                                                 $"Saving execution for {healthCheckItem.ServiceName}");
                                continue;
                            }

                            await transaction.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Health check GRPC execution saving error - Saving execution for {healthCheckItem.ServiceName} - " +
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
                                _logger.LogError($"Health check GRPC failure saving error - " +
                                                 $"Saving failure for {healthCheckItem.ServiceName}");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Health check GRPC saving error - Saving failure for {healthCheckItem.ServiceName} - " +
                                             $"Status : {healthCheckItem.Status} - " +
                                             $"Message : {ex.Message}");

                            transaction.Rollback();
                        }
                    }
                }
            }

            _logger.LogInformation($"Health check GRPC saving finished");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Health check GRPC saving exception error - " +
                             $"Message : {ex.Message}");
        }
    }
}
