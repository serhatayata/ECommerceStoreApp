using Autofac;
using CampaignService.Api.GraphQL.DataLoaders.BatchDataLoaders;
using CampaignService.Api.GraphQL.DataLoaders.CollectionBatchDataLoaders;
using CampaignService.Api.GraphQL.GraphQLSchema;
using CampaignService.Api.Repository.Abstract;
using CampaignService.Api.Repository.Concrete;
using GraphQL.Types;

namespace CampaignService.Api.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CampaignSchema>().As<ISchema>().InstancePerDependency();
            builder.RegisterType<CampaignItemSchema>().As<ISchema>().InstancePerDependency();
            builder.RegisterType<CampaignSourceSchema>().As<ISchema>().InstancePerDependency();
            builder.RegisterType<CampaignRuleSchema>().As<ISchema>().InstancePerDependency();

            builder.RegisterType<CampaignRepository>().As<ICampaignRepository>().InstancePerDependency();
            builder.RegisterType<CampaignItemRepository>().As<ICampaignItemRepository>().InstancePerDependency();
            builder.RegisterType<CampaignRuleRepository>().As<ICampaignRuleRepository>().InstancePerDependency();
            builder.RegisterType<CampaignSourceRepository>().As<ICampaignSourceRepository>().InstancePerDependency();

            //DataLoader
            builder.RegisterType<CampaignBatchDataLoader>().InstancePerLifetimeScope();
            builder.RegisterType<CampaignItemBatchDataLoader>().InstancePerLifetimeScope();
            builder.RegisterType<CampaignSourceBatchDataLoader>().InstancePerLifetimeScope();
            builder.RegisterType<CampaignRuleBatchDataLoader>().InstancePerLifetimeScope();
            builder.RegisterType<CampaignSourceCollectionBatchDataLoader>().InstancePerLifetimeScope();
            builder.RegisterType<CampaignItemCollectionBatchDataLoader>().InstancePerLifetimeScope();
        }
    }
}
