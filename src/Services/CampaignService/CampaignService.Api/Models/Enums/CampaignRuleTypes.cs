namespace CampaignService.Api.Models.Enums;

public enum CampaignRuleTypes : byte
{
    /// <summary>
    /// If campaign data 4 and value 3, that means  4 product is purchased, but 3 will be paid
    /// One of that not be paid must be lowest price product
    /// </summary>
    BuyAPayB = 0,

    /// <summary>
    /// If campaign data 4, value 10 and campaign type percentage
    /// that means if 4 product is purchased, then %10 discount will be used
    /// </summary>
    NProductDiscount = 1
}
