using Microsoft.AspNetCore.Authentication.JwtBearer;
using NotificationService.Api.Models.ExceptionModels;
using NotificationService.Api.Models.IdentityModels;
using NotificationService.Api.Models.Settings;
using NotificationService.Api.Services.Token.Abstract;
using System.Net.Http.Headers;

namespace NotificationService.Api.Infrastructure.DelegatingHandlers;

public class LocalizationAuthorizationDelegatingHandler : DelegatingHandler
{
    readonly string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    private readonly IClientCredentialsTokenService _clientCredentialTokenService;
    private readonly IConfiguration _configuration;

    public LocalizationAuthorizationDelegatingHandler(IClientCredentialsTokenService clientCredentialTokenService,
                                                      IConfiguration configuration)
    {
        _clientCredentialTokenService = clientCredentialTokenService;
        _configuration = configuration;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var serviceInformation = _configuration.GetSection($"ServiceInformation:{this.env}:LocalizationService").Get<ServiceInformationSettings>();

        var monitoringTokenModel = new ClientCredentialsTokenModel()
        {
            ClientId = serviceInformation.ClientId,
            ClientSecret = serviceInformation.ClientSecret,
            Scope = serviceInformation.Scope
        };

        var token = await _clientCredentialTokenService.GetToken(monitoringTokenModel);

        request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token.Data);
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            throw new UnAuthorizeException();

        return response;
    }
}