﻿using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Data.Repositories.EntityFramework.Concrete;

public class EfBrandRepository : IEfBrandRepository
{
    public Task<Result> AddAsync(Brand entity)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(IntModel model)
    {
        throw new NotImplementedException();
    }

    public Task<DataResult<IReadOnlyList<Brand>>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<DataResult<Brand>> GetAsync(IntModel model)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateAsync(Brand entity)
    {
        throw new NotImplementedException();
    }
}
