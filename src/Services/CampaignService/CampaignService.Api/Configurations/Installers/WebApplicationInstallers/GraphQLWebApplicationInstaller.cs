using CampaignService.Api.GraphQL.Schemas;
using GraphQL.Server.Ui.Playground;

namespace CampaignService.Api.Configurations.Installers.WebApplicationInstallers;

public class GraphQLWebApplicationInstaller : IWebApplicationInstaller
{
    public void Install(WebApplication app, IHostApplicationLifetime lifeTime, IConfiguration configuration)
    {
        //app.UseGraphQL<ISchema>();
        app.UseGraphQL<CampaignSchema>("/api/campaign");
        app.UseGraphQL<CampaignItemSchema>("/api/campaignitem");
        app.UseGraphQL<CampaignSourceSchema>("/api/campaignsource");
        app.UseGraphQL<CampaignRuleSchema>("/api/campaignrule");
        app.UseGraphQL<CouponSchema>("/api/coupon");
        app.UseGraphQL<CouponItemSchema>("/api/couponitem");

        if (app.Environment.IsDevelopment())
        {
            app.UseGraphQLPlayground(
            options: new PlaygroundOptions()
            {
                GraphQLEndPoint = "/api/campaign"
            });
        }
    }
}
