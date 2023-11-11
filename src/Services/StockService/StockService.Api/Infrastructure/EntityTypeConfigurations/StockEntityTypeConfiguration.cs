using MassTransit.Transports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockService.Api.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockService.Api.Infrastructure.EntityTypeConfigurations;

public class StockEntityTypeConfiguration : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.ToTable(name: "Stocks", schema: "stock");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedOnAdd();

        #region SeedData
        var seedData = new[]
        {
            new Stock()
            {
                Id = 1,
                ProductId = 1,
                Count = 1
            },
            new Stock()
            {
                Id = 2,
                ProductId = 2,
                Count = 2
            }
        };
        #endregion

        builder.HasData(seedData);
    }
}
