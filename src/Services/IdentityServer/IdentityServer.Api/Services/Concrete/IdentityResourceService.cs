using AutoMapper;
using IdentityServer.Api.Data.Contexts;
using IdentityServer.Api.Models.ApiScopeModels;
using IdentityServer.Api.Models.Base.Concrete;
using IdentityServer.Api.Models.IdentityResourceModels;
using IdentityServer.Api.Models.IncludeOptions.Account;
using IdentityServer.Api.Services.Abstract;
using IdentityServer.Api.Utilities.Results;
using IdentityServer4.EntityFramework.Mappers;

namespace IdentityServer.Api.Services.Concrete
{
    public class IdentityResourceService : IIdentityResourceService
    {
        private AppConfigurationDbContext _confDbContext;
        private IMapper _mapper;

        public IdentityResourceService(AppConfigurationDbContext confDbContext, IMapper mapper)
        {
            _confDbContext = confDbContext;
            _mapper = mapper;
        }

        public DataResult<IdentityResourceModel> Add(IdentityResourceAddModel model)
        {
            var mappedIdentityResource = _mapper.Map<IdentityServer4.Models.IdentityResource>(model);
            var addedIdentityResource = mappedIdentityResource.ToEntity();

            _confDbContext.IdentityResources.Add(addedIdentityResource);
            var result = _confDbContext.SaveChanges();

            var returnValue = _mapper.Map<IdentityResourceModel>(mappedIdentityResource);
            return result > 0 ? new SuccessDataResult<IdentityResourceModel>(returnValue) : new ErrorDataResult<IdentityResourceModel>();
        }

        public Result Delete(StringModel model)
        {
            var existingIdentityResource = _confDbContext.IdentityResources.FirstOrDefault(id => id.Name == model.Value);
            if (existingIdentityResource == null)
                return new ErrorResult();

            _confDbContext.Remove(existingIdentityResource);
            var clientScopes = _confDbContext.ClientScopes.Where(c => c.Scope == model.Value)?.ToArray();
            if (clientScopes?.Count() > 0)
                _confDbContext.RemoveRange(clientScopes);

            var result = _confDbContext.SaveChanges();
            return result > 0 ? new SuccessResult() : new ErrorResult();
        }

        public DataResult<IdentityResourceModel> Get(StringModel model, IdentityResourceIncludeOptions options)
        {
            var result = _confDbContext.IdentityResources.FirstOrDefault(c => c.Name == model.Value);

            if (result == null)
                return new SuccessDataResult<IdentityResourceModel>();

            if (options.UserClaims)
                _confDbContext.Entry(result).Collection(c => c.UserClaims).Load();
            if (options.Properties)
                _confDbContext.Entry(result).Collection(c => c.Properties).Load();

            var mappedResult = _mapper.Map<IdentityResourceModel>(result);
            return new SuccessDataResult<IdentityResourceModel>(mappedResult);
        }

        public DataResult<List<IdentityResourceModel>> Get(List<string> scopeNames, IdentityResourceIncludeOptions options)
        {
            var result = _confDbContext.IdentityResources.Where(c => scopeNames.Contains(c.Name)).ToList();
            if (result.Count() < 1)
                return new SuccessDataResult<List<IdentityResourceModel>>(new List<IdentityResourceModel>());

            foreach (var apiScope in result)
            {
                if (options.UserClaims)
                    _confDbContext.Entry(apiScope).Collection(c => c.UserClaims).Load();
                if (options.Properties)
                    _confDbContext.Entry(apiScope).Collection(c => c.Properties).Load();
            }

            var mappedResult = _mapper.Map<List<IdentityResourceModel>>(result);
            return new SuccessDataResult<List<IdentityResourceModel>>(mappedResult);
        }

        public DataResult<List<IdentityResourceModel>> GetAll(IdentityResourceIncludeOptions options)
        {
            var result = _confDbContext.IdentityResources.ToList();

            if (result.Count < 1)
                return new SuccessDataResult<List<IdentityResourceModel>>(new List<IdentityResourceModel>());

            result.ForEach(identity =>
            {
                if (options.UserClaims)
                    _confDbContext.Entry(identity).Collection(c => c.UserClaims).Load();
                if (options.Properties)
                    _confDbContext.Entry(identity).Collection(c => c.Properties).Load();
            });

            var mappedResult = _mapper.Map<List<IdentityResourceModel>>(result);
            return new SuccessDataResult<List<IdentityResourceModel>>(mappedResult);
        }
    }
}
