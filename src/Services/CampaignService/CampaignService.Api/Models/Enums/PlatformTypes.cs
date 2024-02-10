namespace CampaignService.Api.Models.Enums;

[Flags]
public enum PlatformTypes
{
    /// <summary>
    /// Available for web
    /// </summary>
    Web = 1,

    /// <summary>
    /// Available for mobile
    /// </summary>
    Mobile = 2
}
