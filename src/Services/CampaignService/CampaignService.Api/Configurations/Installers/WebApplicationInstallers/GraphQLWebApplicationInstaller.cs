using CampaignService.Api.GraphQL.GraphQLSchema;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;

namespace CampaignService.Api.Configurations.Installers.WebApplicationInstallers;

public class GraphQLWebApplicationInstaller : IWebApplicationInstaller
{
    public void Install(WebApplication app, IHostApplicationLifetime lifeTime, IConfiguration configuration)
    {
        app.UseGraphQL<ISchema>();
        app.UseGraphQLPlayground(options: new PlaygroundOptions());

        app.UseGraphQL<CampaignSchema>("/api/campaign");
        app.UseGraphQL<CampaignItemSchema>("/api/campaignitem");
        app.UseGraphQL<CampaignSourceSchema>("/api/campaignsource");
        app.UseGraphQL<CampaignRuleSchema>("/api/campaignrule");
    }
}
