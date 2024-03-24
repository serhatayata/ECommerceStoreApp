using Localization.BackgroundTasks.Attributes;
using Polly;
using System.Reflection;
using System.Text;

namespace Localization.BackgroundTasks.Installers.ServiceInstallers;

[InstallerOrder(Order = 3)]
public class CDCServiceInstaller : IServiceInstaller
{
    public async void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        // CDC debezium connector
        var environment = hostEnvironment.EnvironmentName;
        var uri = configuration.GetSection($"CDCSettings:{hostEnvironment.EnvironmentName}:Uri").Value;

        var rootPath = hostEnvironment.ContentRootPath;
        var fullPath = Path.Combine(rootPath, "Configurations/debezium/debezium-resource-config.json");

        var jsonData = System.IO.File.ReadAllText(fullPath);

        if (jsonData == null)
        {
            Serilog.Log.Error("CDC Error handling message for NULL JSON data: Method : {ClassName}.{MethodName}",
                    nameof(CDCServiceInstaller),
                    MethodBase.GetCurrentMethod()?.Name);

            return;
        }

        var policy = Policy.Handle<Exception>()
        .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
        {
            Serilog.Log.Error("CDC Error handling message: {ExceptionMessage} - Method : {ClassName}.{MethodName}",
                                ex.Message, nameof(CDCServiceInstaller),
                                MethodBase.GetCurrentMethod()?.Name);
        });

        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var httpClientFactory = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();

        await policy.ExecuteAsync(async () =>
        {
            var httpClient = httpClientFactory.CreateClient("apigateway");
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync("cdcconnector/connectors", stringContent);

            if (result.StatusCode == System.Net.HttpStatusCode.Conflict)
                return;

            if (!result.IsSuccessStatusCode)
                throw new Exception("HTTP request not successful");
        });
    }
}
