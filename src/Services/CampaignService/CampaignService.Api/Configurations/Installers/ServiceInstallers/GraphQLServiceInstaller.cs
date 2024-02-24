using CampaignService.Api.GraphQL.Schemas;
using GraphQL;
using GraphQL.DataLoader;
using FluentValidation;
using CampaignService.Api.GraphQL.Validators;
using CampaignService.Api.Attributes;

namespace CampaignService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 5)]
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
                    .AddSchema<CampaignRuleSchema>()
                    .AddSchema<CampaignItemSchema>()
                    .AddSchema<CampaignSourceSchema>()
                    .AddSchema<CampaignSchema>()
                    .AddSchema<CouponSchema>()
                    .AddSchema<CouponItemSchema>()
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
                    .AddGraphTypes(typeof(CampaignRuleSchema).Assembly)
                    .AddGraphTypes(typeof(CouponSchema).Assembly)
                    .AddGraphTypes(typeof(CouponItemSchema).Assembly)
                    .AddDataLoader());

        services.AddValidatorsFromAssemblyContaining<CampaignInputValidator>();
        services.AddValidatorsFromAssemblyContaining<CampaignItemInputValidator>();
        services.AddValidatorsFromAssemblyContaining<CampaignSourceInputValidator>();
        services.AddValidatorsFromAssemblyContaining<CampaignRuleInputValidator>();
        services.AddValidatorsFromAssemblyContaining<CouponInputValidator>();
        services.AddValidatorsFromAssemblyContaining<CouponItemInputValidator>();
    }
}
