using IdentityServer.Api.Attributes;
using IdentityServer.Api.Models.LogModels;
using IdentityServer.Api.Services.ElasticSearch.Abstract;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace IdentityServer.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 7)]
public class LogServiceInstaller : IServiceInstaller
{
    public async void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        //Get the environment which the app is running on
        var env = hostEnvironment.EnvironmentName;

        var serilogConfiguration = new ConfigurationBuilder()
            .AddJsonFile("Configurations/Settings/serilog.json",
                         optional: false,
                         reloadOnChange: true)
            .AddJsonFile($"Configurations/Settings/serilog.{hostEnvironment.EnvironmentName}.json",
                         optional: true,
                         reloadOnChange: true)
            .Build();

        //Create logger
        Log.Logger = new LoggerConfiguration()
                        .Enrich.FromLogContext()
                        .Enrich.WithMachineName()
                        .WriteTo.Async(writeTo => writeTo.Console(new Serilog.Formatting.Json.JsonFormatter()))
                        .WriteTo.Async(writeTo => writeTo.Debug(new RenderedCompactJsonFormatter()))
                        .WriteTo.Async(writeTo => writeTo.Elasticsearch(ConfigureElasticSink(serilogConfiguration, env)))
                        .Enrich.WithProperty("Environment", env)
                        .ReadFrom.Configuration(serilogConfiguration)
                        .CreateLogger();

        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope(); var elasticSearchService = scope.ServiceProvider.GetRequiredService<IElasticSearchService>();

        var elasticLogOptions = configuration.GetSection("ElasticSearchOptions").Get<ElasticSearchOptions>();
        _ = elasticSearchService.CreateIndexAsync<LogDetail>(elasticLogOptions.LogIndex).GetAwaiter().GetResult();
    }

    private static ElasticsearchSinkOptions ConfigureElasticSink(IConfiguration configuration, string environment)
    {
        var connString = configuration.GetSection("ElasticSearchLogOptions:ConnectionString").Value;

        return new ElasticsearchSinkOptions(new Uri(connString))
        {
            AutoRegisterTemplate = true,
            IndexFormat = $"{Assembly.GetExecutingAssembly()?.GetName()?.Name?.ToLowerInvariant().Replace(".", "-")}-{environment?.ToLowerInvariant().Replace(".", "-")}",
            TypeName = null,
            BatchAction = ElasticOpType.Create,
            CustomFormatter = new ElasticsearchJsonFormatter(),
            OverwriteTemplate = true,
            DetectElasticsearchVersion = true,
            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
            NumberOfReplicas = 1,
            NumberOfShards = 2,
            FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
            EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                                       EmitEventFailureHandling.WriteToFailureSink |
                                                       EmitEventFailureHandling.RaiseCallback |
                                                       EmitEventFailureHandling.ThrowException
        };
    }
}
