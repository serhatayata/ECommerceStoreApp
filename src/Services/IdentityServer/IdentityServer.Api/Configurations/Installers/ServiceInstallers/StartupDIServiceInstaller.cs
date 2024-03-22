using IdentityServer.Api.Configurations.Installers;
using IdentityServer.Api.Services.ElasticSearch.Abstract;
using IdentityServer.Api.Services.ElasticSearch.Concrete;
using IdentityServer.Api.Services.Redis.Abstract;
using IdentityServer.Api.Services.Redis.Concrete;
using IdentityServer.Api.Services.Token.Abstract;
using IdentityServer.Api.Services.Token.Concrete;
using IdentityServer.Api.Utilities.Security.Jwt;
using IdentityServer.Api.Attributes;
using IdentityServer.Api.Models.Settings;

namespace IdentityServer.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 1)]
public class StartupDIServiceInstaller : IServiceInstaller
{
    public void Install(
        IServiceCollection services, 
        IConfiguration configuration, 
        IWebHostEnvironment hostEnvironment)
    {
        services.AddSingleton<IElasticSearchService, ElasticSearchService>();
        services.AddSingleton<IRedisService, RedisService>();
        services.AddSingleton<IJwtHelper, JwtHelper>();
        services.AddTransient<IClientCredentialsTokenService, ClientCredentialsTokenService>();

        services.Configure<SourceOrigin>(configuration.GetSection($"SourceOriginSettings:{hostEnvironment.EnvironmentName}"));

        services.AddAccessTokenManagement();
    }
}
