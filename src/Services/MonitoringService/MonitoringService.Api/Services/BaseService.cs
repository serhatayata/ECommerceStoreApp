namespace MonitoringService.Api.Services;

public abstract class BaseService
{
    public string Env { get; set; }
    public BaseService()
    {
        this.Env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    }
}
