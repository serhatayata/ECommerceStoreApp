using Quartz;

namespace Monitoring.BackgroundTasks.Services;

public class HealthCheckSaveJob : IJob
{
    private readonly ILogger<HealthCheckSaveJob> _logger;

    public HealthCheckSaveJob(
        ILogger<HealthCheckSaveJob> logger)
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        try
        {
            _logger.LogInformation("Health check saving started");
            // EXECUTE

            //
            _logger.LogInformation("Health check saving finished");
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Health check saving exception error - Message : {ex.Message}");
            return Task.CompletedTask;
        }
    }
}
