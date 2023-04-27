using AutoMapper;
using IdentityServer.Api.Data.Contexts;
using IdentityServer.Api.Dtos;
using IdentityServer.Api.Services.Abstract;
using IdentityServer.Api.Utilities.Results;
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

        public DataResult<ApiResourceDto> Add(ApiResourceDto model)
        {
            var existingApiResource = _confDbContext.ApiResources.FirstOrDefault(s => s.Name == model.Name);
            if(existingApiResource != null)
                return new ErrorDataResult<ApiResourceDto>();

            var mappedApiResource = _mapper.Map<IdentityServer4.EntityFramework.Entities.ApiResource>(model);
            _confDbContext.ApiResources.Add(mappedApiResource);
            var result = _confDbContext.SaveChanges();
            return result == 1 ? new SuccessDataResult<ApiResourceDto>(model) : new ErrorDataResult<ApiResourceDto>();
        }

        public Result Delete(string name)
        {
            var existingApiResource = _confDbContext.ApiResources.FirstOrDefault(c => c.Name == name);
            if (existingApiResource == null)
                return new ErrorResult();

            _confDbContext.ApiResources.Remove(existingApiResource);
            var result = _confDbContext.SaveChanges();
            return result == 1 ? new SuccessResult() : new ErrorResult();
        }

        public DataResult<List<ApiResourceDto>> GetAll(Models.IncludeOptions.Account.ApiResourceIncludeOptions options)
        {
            var result = _confDbContext.ApiResources.ToList();
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

            var mappedResult = _mapper.Map<List<ApiResourceDto>>(result);
            return new SuccessDataResult<List<ApiResourceDto>>(mappedResult);
        }

        public DataResult<ApiResourceDto> Get(string name, Models.IncludeOptions.Account.ApiResourceIncludeOptions options)
        {
            var result = _confDbContext.ApiResources.FirstOrDefault(c => c.Name == name);
            if (result == null)
                return new SuccessDataResult<ApiResourceDto>();

            if (options.Secrets)
                _confDbContext.Entry(result).Collection(c => c.Secrets).Load();
            if (options.Scopes)
                _confDbContext.Entry(result).Collection(c => c.Scopes).Load();
            if (options.UserClaims)
                _confDbContext.Entry(result).Collection(c => c.UserClaims).Load();
            if (options.Properties)
                _confDbContext.Entry(result).Collection(c => c.Properties).Load();

            var mappedResult = _mapper.Map<ApiResourceDto>(result);
            return new SuccessDataResult<ApiResourceDto>(mappedResult);
        }
    }
}
