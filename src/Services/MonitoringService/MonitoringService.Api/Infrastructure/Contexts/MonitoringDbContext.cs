using Microsoft.EntityFrameworkCore;
using MonitoringService.Api.Entities;
using MonitoringService.Api.Infrastructure.EntityConfigurations;
using MonitoringService.Api.Models.Enums;

namespace MonitoringService.Api.Infrastructure.Contexts;

public class MonitoringDbContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public MonitoringDbContext(
        IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to postgres with connection string from app settings
        options.UseNpgsql(Configuration.GetConnectionString("MonitoringDB"));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasPostgresEnum<HealthCheckStatus>();

        builder.ApplyConfiguration(new HealthCheckConfigurationEntityTypeConfiguration());
        builder.ApplyConfiguration(new HealthCheckExecutionEntryEntityTypeConfiguration());
        builder.ApplyConfiguration(new HealthCheckFailureEntityTypeConfiguration());
        builder.ApplyConfiguration(new HealthCheckExecutionEntityTypeConfiguration());
    }

    public DbSet<HealthCheckConfiguration> HealthCheckConfigurations { get; set; }
    public DbSet<HealthCheckExecutionEntry> HealthCheckExecutionEntries { get; set; }
    public DbSet<HealthCheckFailure> HealthCheckFailures { get; set; }
    public DbSet<HealthCheckExecution> HealthCheckExecutions { get; set; }
}
