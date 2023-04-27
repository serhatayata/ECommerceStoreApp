using AutoMapper;
using IdentityServer.Api.Data.Contexts;
using IdentityServer.Api.Dtos;
using IdentityServer.Api.Models.IncludeOptions.Account;
using IdentityServer.Api.Services.Abstract;
using IdentityServer.Api.Utilities.Results;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Api.Services.Concrete
{
    public class ApiScopeService : IApiScopeService
    {
        private AppConfigurationDbContext _confDbContext;
        private IMapper _mapper;

        public ApiScopeService(AppConfigurationDbContext confDbContext, IMapper mapper)
        {
            _confDbContext = confDbContext;
            _mapper = mapper;
        }

        public DataResult<ApiScopeDto> Add(ApiScopeDto model)
        {
            var mappedApiScope = _mapper.Map<IdentityServer4.EntityFramework.Entities.ApiScope>(model);
            _confDbContext.ApiScopes.Add(mappedApiScope);
            var result = _confDbContext.SaveChanges();
            return result == 1 ? new SuccessDataResult<ApiScopeDto>(model) : new ErrorDataResult<ApiScopeDto>();
        }

        public Result Delete(int id)
        {
            var existingApiScope = _confDbContext.ApiScopes.FirstOrDefault(c => c.Id == id);
            if (existingApiScope == null)
                return new ErrorResult();

            _confDbContext.ApiScopes.Remove(existingApiScope);
            var result = _confDbContext.SaveChanges();
            return result == 1 ? new SuccessResult() : new ErrorResult();
        }

        public DataResult<List<ApiScopeDto>> GetAll(ApiScopeIncludeOptions options)
        {
            var result = _confDbContext.ApiScopes.ToList();
            result.ForEach(apiScope =>
            {
                if (options.UserClaims)
                    _confDbContext.Entry(apiScope).Collection(c => c.UserClaims).Load();
                if (options.Properties)
                    _confDbContext.Entry(apiScope).Collection(c => c.Properties).Load();
            });

            var mappedResult = _mapper.Map<List<ApiScopeDto>>(result);
            return new SuccessDataResult<List<ApiScopeDto>>(mappedResult);
        }

        public DataResult<ApiScopeDto> Get(int id, ApiScopeIncludeOptions options)
        {
            var result = _confDbContext.ApiScopes.FirstOrDefault(c => c.Id == id);

            if (result == null)
                return new SuccessDataResult<ApiScopeDto>();

            if (options.UserClaims)
                _confDbContext.Entry(result).Collection(c => c.UserClaims).Load();
            if (options.Properties)
                _confDbContext.Entry(result).Collection(c => c.Properties).Load();

            var mappedResult = _mapper.Map<ApiScopeDto>(result);
            return new SuccessDataResult<ApiScopeDto>(mappedResult);
        }
    }
}
