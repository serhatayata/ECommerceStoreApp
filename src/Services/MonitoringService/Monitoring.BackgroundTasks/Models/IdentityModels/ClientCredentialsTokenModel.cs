using Monitoring.BackgroundTasks.Utilities.Enums;

namespace Monitoring.BackgroundTasks.Models.IdentityModels;

public class ClientCredentialsTokenModel
{
    public EnumProjectType ProjectType { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public List<string> Scope { get; set; }
}
