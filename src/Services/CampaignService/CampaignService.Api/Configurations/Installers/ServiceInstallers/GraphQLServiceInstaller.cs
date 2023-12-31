using CampaignService.Api.GraphQL.Schemas;
using GraphQL;
using GraphQL.DataLoader;
using FluentValidation;
using CampaignService.Api.GraphQL.Validators;

namespace CampaignService.Api.Configurations.Installers.ServiceInstallers;

public class GraphQLServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddSingleton<IDataLoaderContextAccessor, DataLoaderContextAccessor>();
        services.AddSingleton<DataLoaderDocumentListener>();

        services.AddGraphQL(builder =>
                     builder
                    .AddSystemTextJson()
                    .AddNewtonsoftJson()
                    .AddSchema<CampaignItemSchema>()
                    .AddSchema<CampaignSourceSchema>()
                    .AddSchema<CampaignSchema>()
                    .AddErrorInfoProvider((opts, serviceProvider) =>
                    {
                        opts.ExposeExceptionDetails = true;
                    })
                    .ConfigureExecutionOptions(options =>
                    {
                        options.ThrowOnUnhandledException = true;
                        options.UseFluentValidation(ValidatorCacheBuilder.InstanceDI);
                    })
                    .AddGraphTypes(typeof(CampaignSourceSchema).Assembly)
                    .AddGraphTypes(typeof(CampaignItemSchema).Assembly)
                    .AddGraphTypes(typeof(CampaignSchema).Assembly)
                    .AddDataLoader());

        services.AddValidatorsFromAssemblyContaining<CampaignInputValidator>();
        services.AddValidatorsFromAssemblyContaining<CampaignItemInputValidator>();
        services.AddValidatorsFromAssemblyContaining<CampaignSourceInputValidator>();
    }
}
