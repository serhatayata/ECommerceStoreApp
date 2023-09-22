namespace Web.ApiGateway.Extensions
{
    public static class ConfigurationExtension
    {
        static string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        public static IConfiguration appConfig
        {
            get
            {
                return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"Configurations/appsettings.json", 
                             optional: false, 
                             reloadOnChange: true) // if same value is used like ConnectionString, this will be the second option (optional)
                .AddJsonFile($"Configurations/appsettings.{env}.json", 
                             optional: true, 
                             reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            }
        }

        public static IConfiguration serilogConfig
        {
            get
            {
                return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"Configurations/serilog.json", 
                             optional: false, 
                             reloadOnChange: true) // if same value is used like ConnectionString, this will be the second option (optional)
                .AddJsonFile($"Configurations/serilog.{env}.json", 
                             optional: true, 
                             reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            }
        }
    }
}
