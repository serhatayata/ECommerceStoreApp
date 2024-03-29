using Quartz;
using Localization.BackgroundTasks.Attributes;

namespace Localization.BackgroundTasks.Installers.ServiceInstallers;

[InstallerOrder(Order = 4)]
public class JobServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddQuartz(async q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();

            //var resourceChangeSettings = configuration.GetSection("JobSettings:ResourceChange").Get<ResourceChangeJobSetting>();

            //var resourceChangeJobKey = new JobKey(nameof(ResourceChangeJob), "ResourceChange");

            //q.AddJob<ResourceChangeJob>(opt => opt.WithIdentity(resourceChangeJobKey));

            //q.AddTrigger(cfg => cfg
            //       .ForJob(resourceChangeJobKey)
            //       .WithIdentity(GetJobKey<ResourceChangeJob>())
            //       .StartAt(DateTime.Now.AddSeconds(50))
            //       .WithSimpleSchedule(s => s
            //           .WithIntervalInMinutes(resourceChangeSettings?.Interval ?? 1)
            //           .RepeatForever()));
        });

        services.AddQuartzHostedService(q =>
        {
            q.WaitForJobsToComplete = true;
        });

        return Task.CompletedTask;
    }

    private static string GetJobKey<T>()
    {
        return $"{typeof(T).Name}-Trigger";
    }
}
