using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitoringService.Api.Entities;

namespace MonitoringService.Api.Infrastructure.EntityConfigurations;

public class HealthCheckConfigurationEntityTypeConfiguration : IEntityTypeConfiguration<HealthCheckConfiguration>
{
    public void Configure(EntityTypeBuilder<HealthCheckConfiguration> builder)
    {
        builder.ToTable(
            name: "Configurations",
            schema: "healthcheck"
        );

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Uri).HasColumnType("character varying(200)");

        builder.Property(c => c.Name).HasColumnType("character varying(100)");
    }
}
