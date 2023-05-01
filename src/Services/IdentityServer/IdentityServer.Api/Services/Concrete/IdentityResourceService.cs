using AutoMapper;
using IdentityServer.Api.Data.Contexts;
using IdentityServer.Api.Dtos.ApiScopeDtos;
using IdentityServer.Api.Dtos.Base.Concrete;
using IdentityServer.Api.Dtos.ClientDtos;
using IdentityServer.Api.Dtos.IdentityResourceDtos;
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

        /// <summary>
        /// Add identity resource
        /// </summary>
        /// <param name="model">identity resource add dto</param>
        /// <returns><see cref="DataResult{T}"/></returns>
        public DataResult<IdentityResourceDto> Add(IdentityResourceAddDto model)
        {
            var mappedIdentityResource = _mapper.Map<IdentityServer4.Models.IdentityResource>(model);
            var addedIdentityResource = mappedIdentityResource.ToEntity();

            _confDbContext.IdentityResources.Add(addedIdentityResource);
            var result = _confDbContext.SaveChanges();

            var returnValue = _mapper.Map<IdentityResourceDto>(mappedIdentityResource);
            return result > 0 ? new SuccessDataResult<IdentityResourceDto>(returnValue) : new ErrorDataResult<IdentityResourceDto>();
        }

        /// <summary>
        /// Delete specified identity resource
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Result Delete(StringDto model)
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

        /// <summary>
        /// Get specified identity resource
        /// </summary>
        /// <param name="model">name of identity resource</param>
        /// <param name="options">include options</param>
        /// <returns></returns>
        public DataResult<IdentityResourceDto> Get(StringDto model, IdentityResourceIncludeOptions options)
        {
            var result = _confDbContext.IdentityResources.FirstOrDefault(c => c.Name == model.Value);

            if (result == null)
                return new SuccessDataResult<IdentityResourceDto>();

            if (options.UserClaims)
                _confDbContext.Entry(result).Collection(c => c.UserClaims).Load();
            if (options.Properties)
                _confDbContext.Entry(result).Collection(c => c.Properties).Load();

            var mappedResult = _mapper.Map<IdentityResourceDto>(result);
            return new SuccessDataResult<IdentityResourceDto>(mappedResult);
        }

        /// <summary>
        /// Get all identity resources
        /// </summary>
        /// <param name="options">include options</param>
        /// <returns></returns>
        public DataResult<List<IdentityResourceDto>> GetAll(IdentityResourceIncludeOptions options)
        {
            var result = _confDbContext.IdentityResources.ToList();

            if (result.Count == 0)
                return new SuccessDataResult<List<IdentityResourceDto>>();

            result.ForEach(identity =>
            {
                if (options.UserClaims)
                    _confDbContext.Entry(identity).Collection(c => c.UserClaims).Load();
                if (options.Properties)
                    _confDbContext.Entry(identity).Collection(c => c.Properties).Load();
            });

            var mappedResult = _mapper.Map<List<IdentityResourceDto>>(result);
            return new SuccessDataResult<List<IdentityResourceDto>>(mappedResult);
        }
    }
}
