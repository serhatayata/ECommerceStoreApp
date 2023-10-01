using IdentityModel.Client;
using MonitoringService.Api.Models.IdentityModels;
using MonitoringService.Api.Models.Settings;
using MonitoringService.Api.Services.Token.Abstract;
using MonitoringService.Api.Utilities.Results;
using System.Reflection;

namespace MonitoringService.Api.Services.Token.Concrete;

public class ClientCredentialsTokenService : IClientCredentialsTokenService
{
    readonly string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private readonly ILogger<ClientCredentialsTokenService> _logger;

    public ClientCredentialsTokenService(
        IConfiguration configuration,
        HttpClient httpClient,
        ILogger<ClientCredentialsTokenService> logger)
    {
        _configuration = configuration;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<DataResult<string>> GetToken(ClientCredentialsTokenModel model)
    {
        try
        {
            var identityServerInfo = _configuration.GetSection($"ServiceInformation:{env}:IdentityServer").Get<ServiceInformationSettings>();

            var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = identityServerInfo.Url,
                Policy = new DiscoveryPolicy
                {
                    RequireHttps = false,
                    ValidateIssuerName = false,
                    ValidateEndpoints = false
                }
            });

            if (disco.IsError)
                throw disco?.Exception ?? new Exception("Discovery error");

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
        catch (Exception ex)
        {
            _logger.LogError(ex, "ERROR authorization request message: {ExceptionMessage} - Method : {ClassName}.{MethodName}",
                 ex.Message,
                 nameof(ClientCredentialsTokenService),
                 MethodBase.GetCurrentMethod()?.Name);

            return new ErrorDataResult<string>();
        }
    }
}
