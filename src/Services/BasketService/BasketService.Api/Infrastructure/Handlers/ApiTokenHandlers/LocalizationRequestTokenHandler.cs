using BasketService.Api.Models;
using BasketService.Api.Services.Token.Abstract;
using BasketService.Api.Utilities.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net.Http.Headers;

namespace BasketService.Api.Infrastructure.Handlers.ApiTokenHandlers
{
    public class LocalizationRequestTokenHandler : DelegatingHandler
    {
        private readonly IClientCredentialsTokenService _clientCredentialTokenService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LocalizationRequestTokenHandler> _logger;

        public LocalizationRequestTokenHandler(IClientCredentialsTokenService clientCredentialTokenService,
                                               IConfiguration configuration,
                                               ILogger<LocalizationRequestTokenHandler> logger)
        {
            _clientCredentialTokenService = clientCredentialTokenService;
            _configuration = configuration;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //var localizationStaticScheme = _configuration.GetSection("LocalizationSettings:Scheme").Value;

            try
            {
                var token = await _clientCredentialTokenService.GetToken(EnumProjectType.LocalizationService, ApiPermissionType.ReadPermission);

                request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token.Data);
                var response = await base.SendAsync(request, cancellationToken);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new UnAuthorizeException();
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending request for localization - {appName}", Program.appName);
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
