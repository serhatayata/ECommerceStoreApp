using Localization.BackgroundTasks.Entities.Localization;
using Microsoft.EntityFrameworkCore;

namespace Localization.BackgroundTasks.Infrastructure.Contexts;

public class LocalizationDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public LocalizationDbContext(
        IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("Localization"));
    }

    public DbSet<Resource> Resources { get; set; }
}
