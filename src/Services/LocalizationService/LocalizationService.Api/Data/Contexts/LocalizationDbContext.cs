using LocalizationService.Api.Data.EntityConfigurations;
using LocalizationService.Api.Entities;
using LocalizationService.Api.Utilities.IoC;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LocalizationService.Api.Data.Contexts
{
    public class LocalizationDbContext : DbContext, ILocalizationDbContext
    {
        private readonly IConfiguration? _configuration;

        public LocalizationDbContext(DbContextOptions<LocalizationDbContext> options) : base(options)
        {
            _configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
        }

        public DbSet<Resource> Resources { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Language> Languages { get; set; }

        public IDbConnection Connection => Database.GetDbConnection();

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

            modelBuilder.ApplyConfiguration(new ResourceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MemberEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LanguageEntityTypeConfiguration());
        }
    }
}
