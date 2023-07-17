using CatalogService.Api.Data.EntityConfigurations;
using CatalogService.Api.Entities;
using CatalogService.Api.Entities.Abstract;
using CatalogService.Api.Utilities.IoC;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection;

namespace CatalogService.Api.Data.Contexts
{
    public class CatalogDbContext : DbContext, ICatalogDbContext
    {
        private readonly IConfiguration? _configuration;
        public IDbConnection Connection => Database.GetDbConnection();

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {
            _configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }
        public DbSet<ProductFeatureProperty> ProductFeatureProperties { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Feature> Features { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new BrandEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CommentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FeatureEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductFeatureEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductFeaturePropertyEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductTypeEntityTypeConfiguration());
        }

        public string GetTableNameWithScheme<T>() where T : class, IEntity
        {
            var entityType = Model.FindEntityType(typeof(T));
            var schema = entityType?.GetSchema();
            var tableName = entityType?.GetTableName();
            if (schema == null)
                return entityType?.GetTableName() ?? string.Empty;

            return $"{schema}.{tableName}";
        }

        public string GetKeyColumnName<T>() where T : class, IEntity
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object[] keyAttributes = property.GetCustomAttributes(typeof(KeyAttribute), true);

                if (keyAttributes != null && keyAttributes.Length > 0)
                {
                    object[] columnAttributes = property.GetCustomAttributes(typeof(ColumnAttribute), true);

                    if (columnAttributes != null && columnAttributes.Length > 0)
                    {
                        ColumnAttribute columnAttribute = (ColumnAttribute)columnAttributes[0];
                        return columnAttribute?.Name ?? string.Empty;
                    }
                    else
                    {
                        return property.Name;
                    }
                }
            }

            return null;
        }

        public string GetColumns<T>(bool excludeKey = false) where T : class, IEntity
        {
            var type = typeof(T);
            var columns = string.Join(", ", type.GetProperties()
                .Where(p => !excludeKey || !p.IsDefined(typeof(KeyAttribute)))
                .Select(p =>
                {
                    var columnAttr = p.GetCustomAttribute<ColumnAttribute>();
                    return columnAttr != null ? columnAttr.Name : p.Name;
                }));

            return columns;
        }

        public string GetPropertyNames<T>(bool excludeKey = false) where T : class, IEntity
        {
            var properties = typeof(T).GetProperties()
                .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

            var values = string.Join(", ", properties.Select(p =>
            {
                return $"@{p.Name}";
            }));

            return values;
        }

        public IEnumerable<PropertyInfo> GetProperties<T>(bool excludeKey = false) where T : class, IEntity
        {
            var properties = typeof(T).GetProperties()
                .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

            return properties;
        }

        public string GetKeyPropertyName<T>()
        {
            var properties = typeof(T).GetProperties()
                .Where(p => p.GetCustomAttribute<KeyAttribute>() != null);

            if (properties.Any())
            {
                return properties?.FirstOrDefault()?.Name ?? string.Empty;
            }

            return null;
        }
    }
}
