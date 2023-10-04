using Monitoring.BackgroundTasks.Jobs;
using Monitoring.BackgroundTasks.Models.Settings;
using Quartz;
using System.Runtime.CompilerServices;

namespace Monitoring.BackgroundTasks.Extensions;

public static class JobExtensions
{
    public static async void AddJobConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz(async q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();

            var healthCheckSaveSettings = configuration.GetSection("Settings:HealthCheckSave").Get<HealthCheckSaveSettings>();
            var healthCheckGrpcSaveSettings = configuration.GetSection("Settings:HealthCheckGrpcSave").Get<HealthCheckSaveSettings>();

            var healthCheckJobKey = new JobKey(nameof(HealthCheckSaveJob), "HealthCheck");
            var healthCheckGrpcJobKey = new JobKey(nameof(HealthCheckGrpcSaveJob), "HealthCheck_GRPC");

            q.AddJob<HealthCheckSaveJob>(opt => opt.WithIdentity(healthCheckJobKey));
            q.AddJob<HealthCheckGrpcSaveJob>(opt => opt.WithIdentity(healthCheckGrpcJobKey));

            q.AddTrigger(cfg => cfg
                   .ForJob(healthCheckJobKey)
                   .WithIdentity(GetJobKey<HealthCheckSaveJob>())
                   .StartAt(DateTime.Now.AddSeconds(50))
                   .WithSimpleSchedule(s => s
                       .WithIntervalInMinutes(healthCheckSaveSettings?.Interval ?? 30)
                       .RepeatForever()));

            q.AddTrigger(cfg => cfg
                   .ForJob(healthCheckGrpcJobKey)
                   .WithIdentity(GetJobKey<HealthCheckGrpcSaveJob>())
                   .StartAt(DateTime.Now.AddSeconds(50))
                   .WithSimpleSchedule(s => s
                       .WithIntervalInMinutes(healthCheckGrpcSaveSettings?.Interval ?? 30)
                       .RepeatForever()));
        });
    }

    private static string GetJobKey<T>()
    {
        return $"{typeof(T).Name}-Trigger";
    }
}
