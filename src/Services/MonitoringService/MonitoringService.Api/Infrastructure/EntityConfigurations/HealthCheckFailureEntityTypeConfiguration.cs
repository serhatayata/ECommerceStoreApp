using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitoringService.Api.Entities;

namespace MonitoringService.Api.Infrastructure.EntityConfigurations;

public class HealthCheckFailureEntityTypeConfiguration : IEntityTypeConfiguration<HealthCheckFailure>
{
    public void Configure(EntityTypeBuilder<HealthCheckFailure> builder)
    {
        builder.ToTable(
            name: "Failure",
            schema: "healthcheck"
        );

        builder.HasKey(e => e.Id);

        builder.Property(e => e.ServiceName).HasColumnType("character varying(100)");

        builder.Property(e => e.CreateDate).HasColumnType("date");

        builder.Property(e => e.CreateDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
