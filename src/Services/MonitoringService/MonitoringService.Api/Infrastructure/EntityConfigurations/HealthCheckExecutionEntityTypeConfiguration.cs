using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitoringService.Api.Entities;

namespace MonitoringService.Api.Infrastructure.EntityConfigurations;

public class HealthCheckExecutionEntityTypeConfiguration : IEntityTypeConfiguration<HealthCheckExecution>
{
    public void Configure(EntityTypeBuilder<HealthCheckExecution> builder)
    {
        builder.ToTable(
            name: "Executions",
            schema: "healthcheck"
        );

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Status).HasColumnType("character varying(50)");

        builder.Property(e => e.ExecutionDate).HasColumnType("timestamp with time zone");
        builder.Property(e => e.ExecutionDate).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(e => e.Uri).HasColumnType("character varying(200)");

        builder.Property(e => e.ServiceName).HasColumnType("character varying(100)");
    }
}
