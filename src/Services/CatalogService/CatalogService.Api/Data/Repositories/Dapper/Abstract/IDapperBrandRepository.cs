﻿using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Data.Repositories.Base;
using CatalogService.Api.Entities;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Data.Repositories.Dapper.Abstract;

public interface IDapperBrandRepository : IDapperGenericRepository<Brand, IntModel>
{
    Task<DataResult<IReadOnlyList<Brand>>> GetAllWithProductsAsync();
    Task<DataResult<IReadOnlyList<Brand>>> GetAllPagedAsync(PagingModel model);
}
