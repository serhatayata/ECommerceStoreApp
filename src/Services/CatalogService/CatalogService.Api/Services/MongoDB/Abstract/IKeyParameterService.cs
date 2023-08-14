using CatalogService.Api.Entities.MongoDB;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Services.MongoDB.Base;

namespace CatalogService.Api.Services.MongoDB.Abstract
{
    public interface IKeyParameterService : IMongoDbService<KeyParameter,StringModel>
    {
        Task<KeyParameter> GetByKeyAsync(StringModel key);
    }
}
