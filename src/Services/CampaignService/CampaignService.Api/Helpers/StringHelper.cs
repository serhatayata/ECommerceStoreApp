namespace CampaignService.Api.Helpers;

public static class StringHelper
{
    public static string Capitalize(this string str) => char.ToUpper(str[0]) + str.Substring(1);
}