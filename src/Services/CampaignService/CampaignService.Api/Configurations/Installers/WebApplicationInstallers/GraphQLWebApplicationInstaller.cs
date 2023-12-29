using CampaignService.Api.GraphQL.GraphQLSchema;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;

namespace CampaignService.Api.Configurations.Installers.WebApplicationInstallers;

public class GraphQLWebApplicationInstaller : IWebApplicationInstaller
{
    public void Install(WebApplication app, IHostApplicationLifetime lifeTime, IConfiguration configuration)
    {
        //app.UseGraphQL<ISchema>();
        app.UseGraphQL<CampaignSchema>("/api/campaign");
        app.UseGraphQL<CampaignItemSchema>("/api/campaignitem");
        app.UseGraphQL<CampaignSourceSchema>("/api/campaignsource");

        if (app.Environment.IsDevelopment())
        {
            app.UseGraphQLPlayground(
            options: new PlaygroundOptions()
            {
                GraphQLEndPoint = "/api/campaignitem"
            });
        }
    }
}
