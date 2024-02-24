namespace CampaignService.Api.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class InstallerOrderAttribute : Attribute
{
    public int Order { get; set; }
}
