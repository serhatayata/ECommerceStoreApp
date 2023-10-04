﻿namespace Localization.BackgroundTasks.Extensions;

public class ConfigurationExtension
{
    static string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    public static IConfiguration appConfig
    {
        get
        {
            return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"Configurations/staticFiles/appsettings.json",
                         optional: false,
                         reloadOnChange: true) // if same value is used like ConnectionString, this will be the second option (optional)
            .AddJsonFile($"Configurations/staticFiles/appsettings.{env}.json",
                         optional: false,
                         reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
        }
    }
}
