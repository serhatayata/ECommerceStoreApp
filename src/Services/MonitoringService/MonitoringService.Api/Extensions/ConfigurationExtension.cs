namespace MonitoringService.Api.Extensions;

public static class ConfigurationExtension
{
    static string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    public static IConfiguration appConfig
    {
        get
        {
            return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"Configurations/StaticFiles/appsettings.json", 
                         optional: false, 
                         reloadOnChange: true) // if same value is used like ConnectionString, this will be the second option (optional)
            .AddJsonFile($"Configurations/StaticFiles/appsettings.{env}.json", 
                         optional: false, 
                         reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
        }
    }
}
