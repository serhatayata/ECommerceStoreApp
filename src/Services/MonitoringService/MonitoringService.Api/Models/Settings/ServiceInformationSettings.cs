namespace MonitoringService.Api.Models.Settings;

public class ServiceInformationSettings
{
    public string Name { get; set; }
    public string Url { get; set; }
    public string UrlSuffix { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public List<string> Scope { get; set; }
}
