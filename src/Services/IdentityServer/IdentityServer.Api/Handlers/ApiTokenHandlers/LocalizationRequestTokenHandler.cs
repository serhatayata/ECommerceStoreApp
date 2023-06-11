﻿using IdentityServer.Api.Models.ExceptionModels;
using IdentityServer.Api.Services.Token.Abstract;
using IdentityServer.Api.Services.Token.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net.Http.Headers;

namespace IdentityServer.Api.Handlers.ApiTokenHandlers
{
    public class LocalizationRequestTokenHandler : DelegatingHandler
    {
        private readonly IClientCredentialsTokenService _clientCredentialTokenService;

        public LocalizationRequestTokenHandler(IClientCredentialsTokenService clientCredentialTokenService)
        {
            _clientCredentialTokenService = clientCredentialTokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _clientCredentialTokenService.GetToken(Utilities.Enums.EnumProjectType.LocalizationService, Models.ClientModels.ApiPermissionType.ReadPermission);

            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token.Data);
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnAuthorizeException();
            }

            return response;
        }
    }
}
