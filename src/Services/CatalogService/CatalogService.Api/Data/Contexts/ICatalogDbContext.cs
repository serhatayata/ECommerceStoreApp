using CatalogService.Api.Entities;
using CatalogService.Api.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data;
using System.Reflection;

namespace CatalogService.Api.Data.Contexts
{
    public interface ICatalogDbContext
    {
        public IDbConnection Connection { get; }
        DatabaseFacade Database { get; }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }
        public DbSet<ProductFeatureProperty> ProductFeatureProperties { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Feature> Features { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        string GetTableNameWithScheme<T>() where T : class, IEntity;
        string GetKeyColumnName<T>() where T : class, IEntity;
        string GetColumns<T>(bool excludeKey = false) where T : class, IEntity;
        string GetPropertyNames<T>(bool excludeKey = false) where T : class, IEntity;
        IEnumerable<PropertyInfo> GetProperties<T>(bool excludeKey = false) where T : class, IEntity;
    }
}
