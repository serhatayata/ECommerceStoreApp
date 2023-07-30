using Autofac;
using CatalogService.Api.Data.Contexts;
using CatalogService.Api.Data.Contexts.Connections.Abstract;
using CatalogService.Api.Data.Contexts.Connections.Concrete;
using CatalogService.Api.Data.Repositories.Base;
using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Data.Repositories.Dapper.Concrete;
using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;
using CatalogService.Api.Data.Repositories.EntityFramework.Concrete;
using CatalogService.Api.Services.Base.Abstract;
using CatalogService.Api.Services.Base.Concrete;

namespace CatalogService.Api.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            #region DB
            builder.RegisterType<CatalogReadDbConnection>().As<ICatalogReadDbConnection>().InstancePerLifetimeScope();
            builder.RegisterType<CatalogWriteDbConnection>().As<ICatalogWriteDbConnection>().InstancePerLifetimeScope();

            builder.Register<ICatalogDbContext>(l => l.Resolve<CatalogDbContext>());
            #endregion
            #region REPOSITORIES
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerDependency();

            builder.RegisterType<DapperBrandRepository>().As<IDapperBrandRepository>().InstancePerDependency();
            builder.RegisterType<DapperCategoryRepository>().As<IDapperCategoryRepository>().InstancePerDependency();
            builder.RegisterType<DapperCommentRepository>().As<IDapperCommentRepository>().InstancePerDependency();
            builder.RegisterType<DapperFeatureRepository>().As<IDapperFeatureRepository>().InstancePerDependency();
            builder.RegisterType<DapperProductRepository>().As<IDapperProductRepository>().InstancePerDependency();

            builder.RegisterType<EfBrandRepository>().As<IEfBrandRepository>().InstancePerDependency();
            builder.RegisterType<EfCategoryRepository>().As<IEfCategoryRepository>().InstancePerDependency();
            builder.RegisterType<EfCommentRepository>().As<IEfCommentRepository>().InstancePerDependency();
            builder.RegisterType<EfFeatureRepository>().As<IEfFeatureRepository>().InstancePerDependency();
            builder.RegisterType<EfProductRepository>().As<IEfProductRepository>().InstancePerDependency();
            #endregion
            #region SERVICES
            builder.RegisterType<BrandService>().As<IBrandService>().InstancePerDependency();
            builder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerDependency();
            builder.RegisterType<CommentService>().As<ICommentService>().InstancePerDependency();
            builder.RegisterType<FeatureService>().As<IFeatureService>().InstancePerDependency();
            builder.RegisterType<ProductService>().As<IProductService>().InstancePerDependency();


            #endregion
        }
    }
}
