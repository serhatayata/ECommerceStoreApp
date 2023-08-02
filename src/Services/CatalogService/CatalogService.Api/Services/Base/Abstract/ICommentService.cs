using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.CommentModels;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Services.Base.Abstract
{
    public interface ICommentService
    {
        Task<Result> AddAsync(CommentAddModel entity);
        Task<Result> UpdateAsync(CommentUpdateModel entity);
        Task<Result> DeleteAsync(IntModel model);
        Task<Result> DeleteByCodeAsync(StringModel model);

        Task<DataResult<CommentModel>> GetAsync(IntModel model);
        Task<DataResult<CommentModel>> GetByCodeAsync(StringModel model);
        Task<DataResult<IReadOnlyList<CommentModel>>> GetAllAsync();
        Task<DataResult<IReadOnlyList<CommentModel>>> GetAllPagedAsync(PagingModel model);
        Task<DataResult<IReadOnlyList<CommentModel>>> GetAllByProductId(IntModel model);
        Task<DataResult<IReadOnlyList<CommentModel>>> GetAllByProductCode(IntModel model);
        Task<DataResult<IReadOnlyList<CommentModel>>> GetAllByUserId(StringModel model);
    }
}
