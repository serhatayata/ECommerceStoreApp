namespace MonitoringService.Api.Services.Base;

public abstract class BaseService
{
    public string Env { get; set; }
    public BaseService()
    {
        Env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    }
}
