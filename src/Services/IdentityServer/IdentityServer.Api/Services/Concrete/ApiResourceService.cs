﻿using AutoMapper;
using IdentityServer.Api.Data.Contexts;
using IdentityServer.Api.Dtos.ApiResourceDtos;
using IdentityServer.Api.Dtos.Base.Concrete;
using IdentityServer.Api.Dtos.ClientDtos;
using IdentityServer.Api.Services.Abstract;
using IdentityServer.Api.Utilities.Results;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Stores;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Api.Services.Concrete
{
    public class ApiResourceService : IApiResourceService
    {
        private AppConfigurationDbContext _confDbContext;
        private readonly IResourceStore _resourceStore;
        private IMapper _mapper;

        public ApiResourceService(AppConfigurationDbContext confDbContext, IResourceStore resourceStore,IMapper mapper)
        {
            _confDbContext = confDbContext;
            _resourceStore = resourceStore;
            _mapper = mapper;
        }

        /// <summary>
        /// Add api resource
        /// </summary>
        /// <param name="model">api resource model dto</param>
        /// <returns><see cref="{T}"/></returns>
        public DataResult<ApiResourceDto> Add(ApiResourceAddDto model)
        {
            var existingApiResource = _confDbContext.ApiResources.FirstOrDefault(s => s.Name == model.Name);
            if(existingApiResource != null)
                return new ErrorDataResult<ApiResourceDto>();

            var allApiScopes = _confDbContext.ApiScopes.Select(s => s.Name).ToList();
            foreach (var scope in model.Scopes)
            {
                if (!allApiScopes.Any(s => s == scope))
                    return new ErrorDataResult<ApiResourceDto>();
            }

            var mappedApiResource = _mapper.Map<IdentityServer4.Models.ApiResource>(model);
            var addedApiResource= mappedApiResource.ToEntity();
            addedApiResource.Secrets.ForEach(c =>
            {
                c.Value = c.Value.Sha256();
            });
            _confDbContext.ApiResources.Add(addedApiResource);
            var result = _confDbContext.SaveChanges();

            var returnValue = _mapper.Map<ApiResourceDto>(mappedApiResource);
            return result > 0 ? new SuccessDataResult<ApiResourceDto>(model) : new ErrorDataResult<ApiResourceDto>();
        }

        /// <summary>
        /// Deletes the specified api resource
        /// </summary>
        /// <param name="model">string model for api resource name</param>
        /// <returns><see cref="{T}"/></returns>
        public Result Delete(StringDto model)
        {
            var existingApiResource = _confDbContext.ApiResources.FirstOrDefault(c => c.Name == model.Value);
            if (existingApiResource == null)
                return new ErrorResult();

            _confDbContext.ApiResources.Remove(existingApiResource);
            var result = _confDbContext.SaveChanges();
            return result > 0 ? new SuccessResult() : new ErrorResult();
        }

        /// <summary>
        /// Get all api resources
        /// </summary>
        /// <param name="options">include options for api resources</param>
        /// <returns><see cref="{T}"/></returns>
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

        /// <summary>
        /// Get api resource by name
        /// </summary>
        /// <param name="model">api resource name</param>
        /// <param name="options">api resource include option</param>
        /// <returns><see cref="{T}"/></returns>
        public DataResult<ApiResourceDto> Get(StringDto model, Models.IncludeOptions.Account.ApiResourceIncludeOptions options)
        {
            var result = _confDbContext.ApiResources.FirstOrDefault(c => c.Name == model.Value);
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
