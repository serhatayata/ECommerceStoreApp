using AutoMapper;
using IdentityServer.Api.Data.Contexts;
using IdentityServer.Api.Dtos.ApiScopeDtos;
using IdentityServer.Api.Dtos.Base.Concrete;
using IdentityServer.Api.Dtos.ClientDtos;
using IdentityServer.Api.Models.IncludeOptions.Account;
using IdentityServer.Api.Services.Abstract;
using IdentityServer.Api.Utilities.Results;
using IdentityServer4.EntityFramework.Mappers;
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

        /// <summary>
        /// Add api scope
        /// </summary>
        /// <param name="model">Api scope add dto</param>
        /// <returns><see cref="DataResult{T}"/></returns>
        public DataResult<ApiScopeDto> Add(ApiScopeAddDto model)
        {
            var mappedApiScope = _mapper.Map<IdentityServer4.Models.ApiScope>(model);
            var addedApiScope = mappedApiScope.ToEntity();

            _confDbContext.ApiScopes.Add(addedApiScope);
            var result = _confDbContext.SaveChanges();

            var returnValue = _mapper.Map<ApiScopeDto>(mappedApiScope);
            return result > 0 ? new SuccessDataResult<ApiScopeDto>(returnValue) : new ErrorDataResult<ApiScopeDto>();
        }

        /// <summary>
        /// Deletes the specified api scope
        /// </summary>
        /// <param name="model">string dto for api scope name</param>
        /// <returns><see cref="Result"/></returns>
        public Result Delete(StringDto model)
        {
            var existingApiScope = _confDbContext.ApiScopes.FirstOrDefault(c => c.Name == model.Value);
            if (existingApiScope == null)
                return new ErrorResult();

            _confDbContext.ApiScopes.Remove(existingApiScope);
            var clientScopes = _confDbContext.ClientScopes.Where(c => c.Scope == model.Value)?.ToArray();
            if (clientScopes?.Count() > 0)
                _confDbContext.RemoveRange(clientScopes);

            var result = _confDbContext.SaveChanges();
            return result > 0 ? new SuccessResult() : new ErrorResult();
        }

        /// <summary>
        /// Get all api scopes
        /// </summary>
        /// <param name="options">get include options for api scopes</param>
        /// <returns></returns>
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

        /// <summary>
        /// Get specified api scope
        /// </summary>
        /// <param name="model">name of api scope</param>
        /// <param name="options">get options for the api scope</param>
        /// <returns></returns>
        public DataResult<ApiScopeDto> Get(StringDto model, ApiScopeIncludeOptions options)
        {
            var result = _confDbContext.ApiScopes.FirstOrDefault(c => c.Name == model.Value);

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
