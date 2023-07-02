using CatalogService.Api.Data.Repositories.Base;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;

namespace CatalogService.Api.Data.Repositories.EntityFramework.Abstract;

public interface IEfCategoryRepository : IGenericRepository<Category, IntModel>
{
}
