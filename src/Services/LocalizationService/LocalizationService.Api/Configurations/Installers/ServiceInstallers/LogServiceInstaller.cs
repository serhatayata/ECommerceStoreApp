using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace LocalizationService.Api.Configurations.Installers.ServiceInstallers;

public class LogServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddLogging();

        //Get the environment which the app is running on
        var env = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        //Get Config
        var serilogConfiguration = new ConfigurationBuilder()
                            .AddJsonFile("Configurations/Settings/serilog.json",
                                         optional: false,
                                         reloadOnChange: true)
                            .AddJsonFile($"Configurations/Settings/serilog.{env}.json",
                                         optional: false,
                                         reloadOnChange: true)
                            .Build();

        //Create logger
        Log.Logger = new LoggerConfiguration()
                        .Enrich.FromLogContext()
                        .Enrich.WithMachineName()
                        .WriteTo.Debug()
                        .WriteTo.Console()
                        .WriteTo.Elasticsearch(ConfigureElasticSink(serilogConfiguration, env))
                        .Enrich.WithProperty("Environment", env)
                        .ReadFrom.Configuration(serilogConfiguration)
                        .CreateLogger();
    }

    private static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
    {
        var connString = configuration.GetSection("ElasticSearchLogOptions:ConnectionString").Value;

        return new ElasticsearchSinkOptions(new Uri(connString))
        {
            AutoRegisterTemplate = true,
            IndexFormat = $"{Assembly.GetExecutingAssembly()?.GetName()?.Name?.ToLowerInvariant().Replace(".", "-")}-{environment?.ToLowerInvariant().Replace(".", "-")}"
        };
    }
}
