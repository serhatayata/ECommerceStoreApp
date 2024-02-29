namespace CampaignService.Api.Models.Settings;

public class LocalizationSettings
{
    public string ConnectionString { get; set; }
    public string DbName { get; set; }
    public int DatabaseId { get; set; }
    public string MemberKey { get; set; }
    public int Duration { get; set; }
}
