﻿using LocalizationService.Api.Attributes;
using LocalizationService.Api.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LocalizationService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 5)]
public class DbContextServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        var assembly = typeof(Program).Assembly.GetName().Name;

        string defaultConnString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ILocalizationDbContext, LocalizationDbContext>(options => options.UseSqlServer(defaultConnString, b => b.MigrationsAssembly(assembly)), ServiceLifetime.Scoped);

        var serviceProvider = services.BuildServiceProvider();
        var context = serviceProvider.GetRequiredService<LocalizationDbContext>();

        _ = context.Database.EnsureCreated();

        return Task.CompletedTask;
    }
}
