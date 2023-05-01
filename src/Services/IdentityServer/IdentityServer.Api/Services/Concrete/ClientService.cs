using AutoMapper;
using IdentityServer.Api.Data.Contexts;
using IdentityServer.Api.Models.Base.Concrete;
using IdentityServer.Api.Models.ClientModels;
using IdentityServer.Api.Models.IncludeOptions.Account;
using IdentityServer.Api.Services.Abstract;
using IdentityServer.Api.Utilities.Results;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;

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

        public DataResult<ClientModel> Add(ClientAddModel model)
        {
            var existingClient = _confDbContext.Clients.FirstOrDefault(s => s.ClientId == model.ClientId);
            if (existingClient != null)
                return new ErrorDataResult<ClientModel>();

            var mappedClient = _mapper.Map<IdentityServer4.Models.Client>(model);

            var addedClient = mappedClient.ToEntity();
            addedClient.ClientSecrets.ForEach(c =>
            {
                c.Value = c.Value.Sha256();
            });
            _confDbContext.Clients.Add(addedClient);
            var result = _confDbContext.SaveChanges();

            var returnValue = _mapper.Map<ClientModel>(mappedClient);
            return result > 0 ? new SuccessDataResult<ClientModel>(returnValue) : new ErrorDataResult<ClientModel>();
        }

        public Result Delete(StringModel model)
        {
            var existingClient = _confDbContext.Clients.FirstOrDefault(c => c.ClientId == model.Value);
            if (existingClient == null)
                return new ErrorResult();

            _confDbContext.Clients.Remove(existingClient);
            var result = _confDbContext.SaveChanges();
            return result > 0 ? new SuccessResult() : new ErrorResult();
        }

        public DataResult<List<ClientModel>> GetAll(ClientIncludeOptions options)
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
                    _confDbContext.Entry(client).Collection(c => c.AllowedGrantTypes).Load();
            });

            var mappedResult = _mapper.Map<List<ClientModel>>(result);

            return new SuccessDataResult<List<ClientModel>>(mappedResult);
        }

        public DataResult<ClientModel> Get(StringModel model, ClientIncludeOptions options)
        {
            var result = _confDbContext.Clients.FirstOrDefault(c => c.ClientId == model.Value);

            if (result == null)
                return new SuccessDataResult<ClientModel>();

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

            var mappedResult = _mapper.Map<ClientModel>(result);
            return new SuccessDataResult<ClientModel>(mappedResult);
        }
    }
}
