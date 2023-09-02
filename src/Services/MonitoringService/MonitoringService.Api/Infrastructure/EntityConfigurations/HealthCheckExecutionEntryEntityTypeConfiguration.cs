using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitoringService.Api.Entities;

namespace MonitoringService.Api.Infrastructure.EntityConfigurations;

public class HealthCheckExecutionEntryEntityTypeConfiguration : IEntityTypeConfiguration<HealthCheckExecutionEntry>
{
    public void Configure(EntityTypeBuilder<HealthCheckExecutionEntry> builder)
    {
        builder.ToTable(
            name: "ExecutionEntries",
            schema: "healthcheck"
        );

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name).HasColumnType("character varying(100)");

        builder.Property(e => e.Status).HasColumnType("character varying(50)");

        builder.Property(e => e.Duration).HasColumnType("character varying(50)");

        builder.Property(e => e.Tags).HasColumnType("character varying(200)");
    }
}
