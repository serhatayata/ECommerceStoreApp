using AutoMapper;
using IdentityServer.Api.Data.Contexts;
using IdentityServer.Api.Dtos;
using IdentityServer.Api.Models.IncludeOptions.Account;
using IdentityServer.Api.Services.Abstract;
using IdentityServer.Api.Utilities.Results;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Api.Services.Concrete
{
    public class ClientService : IClientService
    {
        private AppConfigurationDbContext _confDbContext;
        private IMapper _mapper;

        public ClientService(AppConfigurationDbContext confDbContext, IMapper mapper)
        {
            _confDbContext = confDbContext;
            _mapper = mapper;
        }

        public DataResult<ClientDto> Add(ClientDto model)
        {
            var mappedClient = _mapper.Map<IdentityServer4.EntityFramework.Entities.Client>(model);
            _confDbContext.Clients.Add(mappedClient);
            var result = _confDbContext.SaveChanges();
            return result == 1 ? new SuccessDataResult<ClientDto>(model) : new ErrorDataResult<ClientDto>();
        }

        public Result Delete(string clientId)
        {
            var existingClient = _confDbContext.Clients.FirstOrDefault(c => c.ClientId == clientId);
            if (existingClient == null)
                return new ErrorResult();

            _confDbContext.Clients.Remove(existingClient);
            var result = _confDbContext.SaveChanges();
            return result == 1 ? new SuccessResult() : new ErrorResult();
        }

        public DataResult<List<ClientDto>> GetAll(ClientIncludeOptions options)
        {
            var result = _confDbContext.Clients.ToList();
            result.ForEach(client =>
            {
                if (options.Claims)
                    _confDbContext.Entry(client).Collection(c => c.Claims).Load();
                if (options.Secrets)
                    _confDbContext.Entry(client).Collection(c => c.ClientSecrets).Load();
                if (options.Scopes)
                    _confDbContext.Entry(client).Collection(c => c.AllowedScopes).Load();
                if (options.Properties)
                    _confDbContext.Entry(client).Collection(c => c.Properties).Load();
                if (options.AllowedCorsOrigins)
                    _confDbContext.Entry(client).Collection(c => c.AllowedCorsOrigins).Load();
                if (options.IdentityProviderRestrictions)
                    _confDbContext.Entry(client).Collection(c => c.IdentityProviderRestrictions).Load();
                if (options.PostLogoutRedirectUris)
                    _confDbContext.Entry(client).Collection(c => c.PostLogoutRedirectUris).Load();
                if (options.RedirectUris)
                    _confDbContext.Entry(client).Collection(c => c.RedirectUris).Load();
                if (options.GrantTypes)
                    _confDbContext.Entry(client).Collection(c => c.RedirectUris).Load();
            });

            var mappedResult = _mapper.Map<List<ClientDto>>(result);

            return new SuccessDataResult<List<ClientDto>>(mappedResult);
        }

        public DataResult<ClientDto> Get(string clientId, ClientIncludeOptions options)
        {
            var result = _confDbContext.Clients.FirstOrDefault(c => c.ClientId == clientId);

            if (result == null)
                return new SuccessDataResult<ClientDto>();

            if (options.Claims)
                _confDbContext.Entry(result).Collection(c => c.Claims).Load();
            if (options.Secrets)
                _confDbContext.Entry(result).Collection(c => c.ClientSecrets).Load();
            if (options.Scopes)
                _confDbContext.Entry(result).Collection(c => c.AllowedScopes).Load();
            if (options.Properties)
                _confDbContext.Entry(result).Collection(c => c.Properties).Load();
            if (options.AllowedCorsOrigins)
                _confDbContext.Entry(result).Collection(c => c.AllowedCorsOrigins).Load();
            if (options.IdentityProviderRestrictions)
                _confDbContext.Entry(result).Collection(c => c.IdentityProviderRestrictions).Load();
            if (options.PostLogoutRedirectUris)
                _confDbContext.Entry(result).Collection(c => c.PostLogoutRedirectUris).Load();
            if (options.RedirectUris)
                _confDbContext.Entry(result).Collection(c => c.RedirectUris).Load();
            if (options.GrantTypes)
                _confDbContext.Entry(result).Collection(c => c.AllowedGrantTypes).Load();

            var mappedResult = _mapper.Map<ClientDto>(result);
            return new SuccessDataResult<ClientDto>(mappedResult);
        }
    }
}
