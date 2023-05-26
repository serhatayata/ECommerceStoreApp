using Serilog;

namespace Web.ApiGateway.Extensions
{
    public static class HostExtensions
    {
        public static void AddHostExtensions(this IHostBuilder host, IWebHostEnvironment environment)
        {
            host.UseDefaultServiceProvider((context, options) =>
            {
                options.ValidateOnBuild = false;
                options.ValidateScopes = false;
            });

            host.ConfigureAppConfiguration(config =>
            {
                config.AddJsonFile($"Configurations/ocelot.json")
                      .AddEnvironmentVariables()
                      .Build();
            })
            .ConfigureLogging(s => s.ClearProviders()) // Remove all added providers before
                                                       // https://github.com/serilog/serilog-aspnetcore
            .UseSerilog();
        }
    }
}
