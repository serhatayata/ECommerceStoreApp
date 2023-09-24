namespace MonitoringService.Api.Models.IdentityModels;

public class ClientCredentialsTokenModel
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public List<string> Scope { get; set; }
}
