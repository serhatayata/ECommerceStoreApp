﻿using Localization.BackgroundTasks.Attributes;
using Localization.BackgroundTasks.Services.BackgroundServices;
using Localization.BackgroundTasks.Services.Cache.Abstract;

namespace Localization.BackgroundTasks.Installers.ServiceInstallers;

[InstallerOrder(Order = 1)]
public class StartupDIServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        // Background service
        services.AddSingleton<ResourceChangeBackgroundService>();
        // Scoped
        services.AddSingleton<IRedisService, RedisService>();
    }
}
