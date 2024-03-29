﻿using Monitoring.BackgroundTasks.Attributes;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace Monitoring.BackgroundTasks.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 4)]
public class LogServiceInstaller : IServiceInstaller
{
    public Task Install(
        IServiceCollection services, 
        IConfiguration configuration, 
        IWebHostEnvironment hostEnvironment)
    {
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
                        .WriteTo.Async(writeTo => writeTo.Console(new Serilog.Formatting.Json.JsonFormatter()))
                        .WriteTo.Async(writeTo => writeTo.Debug(new RenderedCompactJsonFormatter()))
                        .WriteTo.Async(writeTo => writeTo.Elasticsearch(ConfigureElasticSink(serilogConfiguration, env)))
                        .Enrich.WithProperty("Environment", env)
                        .ReadFrom.Configuration(serilogConfiguration)
                        .CreateLogger();

        return Task.CompletedTask;
    }

    private static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
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
