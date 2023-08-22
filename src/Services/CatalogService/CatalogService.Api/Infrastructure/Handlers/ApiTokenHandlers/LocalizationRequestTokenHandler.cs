using CatalogService.Api.Services.Token.Abstract;
using CatalogService.Api.Utilities.Enums;
using CatalogService.Api.Utilities.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net.Http.Headers;

namespace CatalogService.Api.Infrastructure.Handlers.ApiTokenHandlers;

public class LocalizationRequestTokenHandler : DelegatingHandler
{
    private readonly IClientCredentialsTokenService _clientCredentialTokenService;
    private readonly IConfiguration _configuration;

    public LocalizationRequestTokenHandler(IClientCredentialsTokenService clientCredentialTokenService,
                                           IConfiguration configuration)
    {
        _clientCredentialTokenService = clientCredentialTokenService;
        _configuration = configuration;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        //var localizationStaticScheme = _configuration.GetSection("LocalizationSettings:Scheme").Value;

        var token = await _clientCredentialTokenService.GetToken(EnumProjectType.LocalizationService, ApiPermissionType.ReadPermission);

        request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token.Data);
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            throw new UnAuthorizeException();
        }

        return response;
    }
}
