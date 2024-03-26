using Ocelot.Values;
using Serilog;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;
using Web.ApiGateway.Attributes;
using Web.ApiGateway.Models.LogModels;
using Web.ApiGateway.Services.ElasticSearch.Abstract;

namespace Web.ApiGateway.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 7)]
public class LogServiceInstaller : IServiceInstaller
{
    public async Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        var env = hostEnvironment.EnvironmentName;

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

        await CreateElasticLogIndex(services, configuration);
    }

    private ElasticsearchSinkOptions ConfigureElasticSink(IConfiguration configuration, string environment)
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

    private async Task CreateElasticLogIndex(IServiceCollection services, IConfiguration configuration)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

        var elasticSearchService = scope.ServiceProvider.GetRequiredService<IElasticSearchService>();

        var elasticLogOptions = configuration.GetSection("ElasticSearchOptions").Get<ElasticSearchOptions>();
        await elasticSearchService.CreateIndexAsync<LogDetail>(elasticLogOptions.LogIndex);
    }
}
