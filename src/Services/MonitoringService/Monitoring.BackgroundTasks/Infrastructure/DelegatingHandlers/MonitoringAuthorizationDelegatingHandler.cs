using Microsoft.AspNetCore.Authentication.JwtBearer;
using Monitoring.BackgroundTasks.Models.ExceptionModels;
using Monitoring.BackgroundTasks.Models.IdentityModels;
using Monitoring.BackgroundTasks.Models.Settings;
using Monitoring.BackgroundTasks.Services.Abstract;
using Monitoring.BackgroundTasks.Utilities.Enums;
using System.Net.Http.Headers;

namespace Monitoring.BackgroundTasks.Infrastructure.DelegatingHandlers;

public class MonitoringAuthorizationDelegatingHandler : DelegatingHandler
{
    readonly string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    private readonly IClientCredentialsTokenService _clientCredentialTokenService;
    private readonly IConfiguration _configuration;

    public MonitoringAuthorizationDelegatingHandler(IClientCredentialsTokenService clientCredentialTokenService,
                                           IConfiguration configuration)
    {
        _clientCredentialTokenService = clientCredentialTokenService;
        _configuration = configuration;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var serviceInformation = _configuration.GetSection($"ServiceInformation:{this.env}:{EnumProjectType.MonitoringService}").Get<ServiceInformationSettings>();

        var monitoringTokenModel = new ClientCredentialsTokenModel()
        {
            ProjectType = EnumProjectType.MonitoringService,
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
