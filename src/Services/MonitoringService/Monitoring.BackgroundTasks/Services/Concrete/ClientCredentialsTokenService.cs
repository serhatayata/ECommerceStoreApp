using IdentityModel.Client;
using Monitoring.BackgroundTasks.Models.IdentityModels;
using Monitoring.BackgroundTasks.Models.Settings;
using Monitoring.BackgroundTasks.Services.Abstract;
using Monitoring.BackgroundTasks.Utilities.Enums;
using Monitoring.BackgroundTasks.Utilities.Results;

namespace Monitoring.BackgroundTasks.Services.Concrete;

public class ClientCredentialsTokenService : IClientCredentialsTokenService
{
    readonly string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public ClientCredentialsTokenService(IConfiguration configuration,
                                     HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task<DataResult<string>> GetToken(ClientCredentialsTokenModel model)
    {
        var identityServerInfo = _configuration.GetSection($"ServiceInformation:{this.env}:{EnumProjectType.IdentityServer}").Get<ServiceInformationSettings>();

        var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
        {
            Address = identityServerInfo.Url,
            Policy = new DiscoveryPolicy { RequireHttps = false }
        });

        if (disco.IsError)
            throw disco.Exception;

        var clientCredentialTokenRequest = new ClientCredentialsTokenRequest
        {
            ClientId = model.ClientId,
            ClientSecret = model.ClientSecret,
            Address = disco.TokenEndpoint,
            GrantType = "client_credentials",
            Scope = string.Join(',', model.Scope)
        };

        var newToken = await _httpClient.RequestClientCredentialsTokenAsync(clientCredentialTokenRequest);
        if (newToken.IsError)
            throw newToken.Exception;

        return new SuccessDataResult<string>(newToken.AccessToken);
    }
}
