namespace NotificationService.Api.Models.Settings;

public class ConsulSettings
{
    public string Address { get; set; }
    public string ServiceName { get; set; }
    public string ServiceId { get; set; }
    public HealthCheckSetting HealthCheckSettings { get; set; }
}

public class HealthCheckSetting
{
    public string UrlPath { get; set; }
    public int HealthCheckIntervalSeconds { get; set; }
    public int HealthCheckTimeoutSeconds { get; set; }
}