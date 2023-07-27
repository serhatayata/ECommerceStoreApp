using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Data.Repositories.Base;
using CatalogService.Api.Entities;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Data.Repositories.Dapper.Abstract
{
    public interface IDapperCommentRepository : IGenericRepository<Comment, IntModel>
    {
        Task<DataResult<Comment>> GetByCodeAsync(StringModel model);
        Task<DataResult<IReadOnlyList<Comment>>> GetAllByProductId(IntModel model);
        Task<DataResult<IReadOnlyList<Comment>>> GetAllPagedAsync(PagingModel model);
        Task<DataResult<IReadOnlyList<Comment>>> GetAllByProductCode(IntModel model);
        Task<DataResult<IReadOnlyList<Comment>>> GetAllByUserId(StringModel model);
        Task<Result> DeleteByCodeAsync(StringModel model);
        Task<Result> UpdateByCodeAsync(Comment entity);
    }
}
