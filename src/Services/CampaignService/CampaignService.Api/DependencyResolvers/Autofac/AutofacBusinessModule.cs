﻿using Autofac;
using CampaignService.Api.GraphQL.DataLoaders.BatchDataLoaders;
using CampaignService.Api.GraphQL.DataLoaders.CollectionBatchDataLoaders;
using CampaignService.Api.GraphQL.Schemas;
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

            builder.RegisterType<CampaignRepository>().As<ICampaignRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CampaignItemRepository>().As<ICampaignItemRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CampaignSourceRepository>().As<ICampaignSourceRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CampaignRuleRepository>().As<ICampaignSourceRepository>().InstancePerLifetimeScope();

            //DataLoader
            builder.RegisterType<CampaignBatchDataLoader>().InstancePerLifetimeScope();
            builder.RegisterType<CampaignItemBatchDataLoader>().InstancePerLifetimeScope();
            builder.RegisterType<CampaignSourceBatchDataLoader>().InstancePerLifetimeScope();
            builder.RegisterType<CampaignSourceCollectionBatchDataLoader>().InstancePerLifetimeScope();
            builder.RegisterType<CampaignItemCollectionBatchDataLoader>().InstancePerLifetimeScope();
        }
    }
}
