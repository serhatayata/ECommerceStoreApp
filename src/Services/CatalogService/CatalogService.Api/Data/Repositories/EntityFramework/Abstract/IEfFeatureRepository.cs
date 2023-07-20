using CatalogService.Api.Data.Repositories.Base;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Data.Repositories.EntityFramework.Abstract;

public interface IEfFeatureRepository : IGenericRepository<Feature, IntModel>
{
    Task<DataResult<IReadOnlyList<Feature>>> GetAllWithProductFeaturesAsync();
}
