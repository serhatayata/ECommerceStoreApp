using CatalogService.Api.Data.EntityConfigurations;
using CatalogService.Api.Entities;
using CatalogService.Api.Entities.Abstract;
using CatalogService.Api.Utilities.IoC;
using Microsoft.EntityFrameworkCore;
using System.Data;

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
    }
}
