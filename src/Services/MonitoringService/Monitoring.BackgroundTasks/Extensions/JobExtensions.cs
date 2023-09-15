using Monitoring.BackgroundTasks.Models.Settings;
using Monitoring.BackgroundTasks.Services.Concrete;
using Quartz;

namespace Monitoring.BackgroundTasks.Extensions;

public static class JobExtensions
{
    public static void AddJobConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();

            #region HealthCheckSave
            var healthCheckSaveSettings = configuration.GetValue<HealthCheckSaveSettings>("Settings:HealthCheckSave");

            var healthCheckJobKey = new JobKey(nameof(HealthCheckSaveJob));
            q.AddJob<HealthCheckSaveJob>(opt => opt.WithIdentity(healthCheckJobKey));

            q.AddTrigger(cfg => 
                cfg.ForJob(healthCheckJobKey)
                   .WithIdentity($"{nameof(HealthCheckSaveJob)}-Trigger")
                   .WithSimpleSchedule(s => 
                          s.WithIntervalInHours(healthCheckSaveSettings?.Interval ?? 12)
                         .RepeatForever()));
            #endregion
        });

    }
}
