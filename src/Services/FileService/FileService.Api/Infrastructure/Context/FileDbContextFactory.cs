using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FileService.Api.Infrastructure.Context;

public class FileDbContextFactory : IDesignTimeDbContextFactory<FileDbContext>
{
    public FileDbContext CreateDbContext(string[] args)
    {
        string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").ToLower();

        // Build config
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"Configurations/Settings/appsettings.{environment}.json")
            .AddEnvironmentVariables()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<FileDbContext>();
        var connectionString = config.GetConnectionString("FileDB");
        optionsBuilder.UseSqlServer(connectionString);
        return new FileDbContext(config);   
    }
}
