using CampaignService.Api.Entities;
using CampaignService.Api.GraphQL.GraphQLTypes.Enums;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.GraphQLTypes;

public class CampaignItemType : ObjectGraphType<CampaignItem>
{
    public CampaignItemType()
    {
        Field(o => o.Id, type: typeof(IdGraphType)).Description("Id property from the campaign item object");
        Field(o => o.CampaignId).Description("Campaign Id property from the campaign item");
        Field(o => o.UserId).Description("User Id property from the campaign item");
        Field(o => o.Description).Description("Description property from the campaign item");
        Field(o => o.Code).Description("Code property from the campaign item");
        Field<CampaignItemStatusEnumType>("status").Resolve(context => context.Source.Status);
        Field(o => o.CreationDate).Description("Creation date property from the campaign item");
        Field(o => o.ExpirationDate).Description("Expiration date property from the campaign item");
    }
}
