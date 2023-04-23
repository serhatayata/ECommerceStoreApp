using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Api.Data.EntityConfigurations.PersistedGrants
{
    public class DeviceCodeEntityTypeConfiguration : IEntityTypeConfiguration<DeviceFlowCodes>
    {
        public void Configure(EntityTypeBuilder<DeviceFlowCodes> builder)
        {
            builder.ToTable(name: "DeviceCodes", schema: "persisted");
        }
    }
}
