﻿using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Data.Repositories.EntityFramework.Concrete;

public class EfProductRepository : IEfProductRepository
{
    public Task<Result> AddAsync(Product entity)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(IntModel model)
    {
        throw new NotImplementedException();
    }

    public Task<DataResult<IReadOnlyList<Product>>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<DataResult<Product>> GetAsync(IntModel model)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateAsync(Product entity)
    {
        throw new NotImplementedException();
    }
}
