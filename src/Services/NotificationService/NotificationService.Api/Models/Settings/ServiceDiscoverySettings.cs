namespace NotificationService.Api.Models.Settings;

public class ServiceDiscoverySettings
{
    public string Address { get; set; }
    public string ServiceName { get; set; }
    public string ServiceId { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
}