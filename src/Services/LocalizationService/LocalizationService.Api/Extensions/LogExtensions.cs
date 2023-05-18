using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace LocalizationService.Api.Extensions
{
    public static class LogExtensions
    {
        public static void AddElasticSearchConfiguration(this IServiceCollection services)
        {
            //Get the environment which the app is running on
            var env = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            //Get Config
            var configuration = new ConfigurationBuilder()
                                .AddJsonFile("Configurations/serilog.json", optional: false, reloadOnChange: true)
                                .AddJsonFile($"Configurations/serilog.{env}.json", optional: true)
                                .Build();

            //Create logger
            Log.Logger = new LoggerConfiguration()
                            .Enrich.FromLogContext()
                            .Enrich.WithMachineName()
                            .WriteTo.Debug()
                            .WriteTo.Console()
                            .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, env))
                            .Enrich.WithProperty("Environment", env)
                            .ReadFrom.Configuration(configuration)
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
}
