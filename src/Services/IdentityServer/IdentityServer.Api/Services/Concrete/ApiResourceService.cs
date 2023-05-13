using AutoMapper;
using IdentityServer.Api.Data.Contexts;
using IdentityServer.Api.Models.ApiResourceModels;
using IdentityServer.Api.Models.Base.Concrete;
using IdentityServer.Api.Models.IncludeOptions.Account;
using IdentityServer.Api.Services.Abstract;
using IdentityServer.Api.Utilities.Results;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Api.Services.Concrete
{
    public class ApiResourceService : IApiResourceService
    {
        private AppConfigurationDbContext _confDbContext;
        private IMapper _mapper;

        public ApiResourceService(AppConfigurationDbContext confDbContext, IMapper mapper)
        {
            _confDbContext = confDbContext;
            _mapper = mapper;
        }

        public DataResult<ApiResourceModel> Add(ApiResourceAddModel model)
        {
            var existingApiResource = _confDbContext.ApiResources.FirstOrDefault(s => s.Name == model.Name);
            if(existingApiResource != null)
                return new ErrorDataResult<ApiResourceModel>();

            var allApiScopes = _confDbContext.ApiScopes.Select(s => s.Name).ToList();
            foreach (var scope in model.Scopes)
            {
                if (!allApiScopes.Any(s => s == scope))
                    return new ErrorDataResult<ApiResourceModel>();
            }

            var mappedApiResource = _mapper.Map<IdentityServer4.Models.ApiResource>(model);
            var addedApiResource= mappedApiResource.ToEntity();
            addedApiResource.Secrets.ForEach(c =>
            {
                c.Value = c.Value.Sha256();
            });
            _confDbContext.ApiResources.Add(addedApiResource);
            var result = _confDbContext.SaveChanges();

            var returnValue = _mapper.Map<ApiResourceModel>(mappedApiResource);
            return result > 0 ? new SuccessDataResult<ApiResourceModel>(model) : new ErrorDataResult<ApiResourceModel>();
        }

        public Result Delete(StringModel model)
        {
            var existingApiResource = _confDbContext.ApiResources.FirstOrDefault(c => c.Name == model.Value);
            if (existingApiResource == null)
                return new ErrorResult();

            _confDbContext.ApiResources.Remove(existingApiResource);
            var result = _confDbContext.SaveChanges();
            return result > 0 ? new SuccessResult() : new ErrorResult();
        }

        public DataResult<List<ApiResourceModel>> GetAll(Models.IncludeOptions.Account.ApiResourceIncludeOptions options)
        {
            var result = _confDbContext.ApiResources.ToList();
            if (result.Count() < 1)
                return new SuccessDataResult<List<ApiResourceModel>>(new List<ApiResourceModel>());

            result.ForEach(apiResource =>
            {
                if (options.Secrets)
                    _confDbContext.Entry(apiResource).Collection(c => c.Secrets).Load();
                if (options.Scopes)
                    _confDbContext.Entry(apiResource).Collection(c => c.Scopes).Load();
                if (options.UserClaims)
                    _confDbContext.Entry(apiResource).Collection(c => c.UserClaims).Load();
                if (options.Properties)
                    _confDbContext.Entry(apiResource).Collection(c => c.Properties).Load();
            });

            var mappedResult = _mapper.Map<List<ApiResourceModel>>(result);
            return new SuccessDataResult<List<ApiResourceModel>>(mappedResult);
        }

        public DataResult<ApiResourceModel> Get(StringModel model, Models.IncludeOptions.Account.ApiResourceIncludeOptions options)
        {
            var result = _confDbContext.ApiResources.FirstOrDefault(c => c.Name == model.Value);
            if (result == null)
                return new SuccessDataResult<ApiResourceModel>();

            if (options.Secrets)
                _confDbContext.Entry(result).Collection(c => c.Secrets).Load();
            if (options.Scopes)
                _confDbContext.Entry(result).Collection(c => c.Scopes).Load();
            if (options.UserClaims)
                _confDbContext.Entry(result).Collection(c => c.UserClaims).Load();
            if (options.Properties)
                _confDbContext.Entry(result).Collection(c => c.Properties).Load();

            var mappedResult = _mapper.Map<ApiResourceModel>(result);
            return new SuccessDataResult<ApiResourceModel>(mappedResult);
        }

        public DataResult<List<ApiResourceModel>> Get(List<string> apiResources, ApiResourceIncludeOptions options)
        {
            var result = _confDbContext.ApiResources.Where(c => apiResources.Contains(c.Name)).ToList();
            if (result.Count() < 1)
                return new SuccessDataResult<List<ApiResourceModel>>(new List<ApiResourceModel>());

            foreach (var apiResource in result)
            {
                if (options.Secrets)
                    _confDbContext.Entry(apiResource).Collection(c => c.Secrets).Load();
                if (options.Scopes)
                    _confDbContext.Entry(apiResource).Collection(c => c.Scopes).Load();
                if (options.UserClaims)
                    _confDbContext.Entry(apiResource).Collection(c => c.UserClaims).Load();
                if (options.Properties)
                    _confDbContext.Entry(apiResource).Collection(c => c.Properties).Load();
            }

            var mappedResult = _mapper.Map<List<ApiResourceModel>>(result);
            return new SuccessDataResult<List<ApiResourceModel>>(mappedResult);
        }

        public DataResult<List<ApiResourceModel>> GetByApiScopeNames(List<string> apiScopeNames, ApiResourceIncludeOptions options)
        {
            var result = _confDbContext.ApiResources
                            .Include(a => a.Scopes)
                            .Where(c => c.Scopes.Any(s => apiScopeNames.Contains(s.Scope)))
                            .ToList();

            if (result.Count() < 1)
                return new SuccessDataResult<List<ApiResourceModel>>(new List<ApiResourceModel>());

            foreach (var apiResource in result)
            {
                if (options.Secrets)
                    _confDbContext.Entry(apiResource).Collection(c => c.Secrets).Load();
                if (options.Scopes)
                    _confDbContext.Entry(apiResource).Collection(c => c.Scopes).Load();
                if (options.UserClaims)
                    _confDbContext.Entry(apiResource).Collection(c => c.UserClaims).Load();
                if (options.Properties)
                    _confDbContext.Entry(apiResource).Collection(c => c.Properties).Load();
            }

            var mappedResult = _mapper.Map<List<ApiResourceModel>>(result);
            return new SuccessDataResult<List<ApiResourceModel>>(mappedResult);
        }
    }
}
