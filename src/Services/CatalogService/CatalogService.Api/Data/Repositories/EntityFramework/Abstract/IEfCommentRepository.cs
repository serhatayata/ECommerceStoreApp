using CatalogService.Api.Data.Repositories.Base;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Data.Repositories.EntityFramework.Abstract;

public interface IEfCommentRepository : IGenericRepository<Comment, IntModel>
{
    Task<Result> DeleteByCodeAsync(StringModel model);
    Task<Result> UpdateByCodeAsync(Comment entity);
}
