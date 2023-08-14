using AutoMapper;
using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.CommentModels;
using CatalogService.Api.Services.Base.Abstract;
using CatalogService.Api.Services.MongoDB.Abstract;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Services.Base.Concrete
{
    public class CommentService : ICommentService
    {
        private readonly IEfCommentRepository _efCommentRepository;
        private readonly IDapperCommentRepository _dapperCommentRepository;
        private readonly IKeyParameterService _keyParameterService;
        private readonly IMapper _mapper;

        public CommentService(
            IEfCommentRepository efCommentRepository, 
            IDapperCommentRepository dapperCommentRepository, 
            IKeyParameterService keyParameterService,
            IMapper mapper)
        {
            _efCommentRepository = efCommentRepository;
            _dapperCommentRepository = dapperCommentRepository;
            _keyParameterService = keyParameterService;
            _mapper = mapper;
        }

        public async Task<Result> AddAsync(CommentAddModel entity)
        {
            var mappedModel = _mapper.Map<Comment>(entity);

            mappedModel.Code = Guid.NewGuid().ToString();
            mappedModel.UpdateDate = DateTime.Now;

            var result = await _efCommentRepository.AddAsync(mappedModel);
            return result;
        }

        public async Task<Result> UpdateAsync(CommentUpdateModel entity)
        {
            var commentExists = await _dapperCommentRepository.GetByCodeAsync(new StringModel(entity.Code));
            if (commentExists.Success && commentExists.Data?.UserId != entity.UserId)
                return new ErrorResult("Comment user not same");

            var mappedModel = _mapper.Map<Comment>(entity);
            var result = await _efCommentRepository.UpdateAsync(mappedModel);
            return result;
        }

        public async Task<Result> DeleteAsync(IntModel model)
        {
            var result = await _efCommentRepository.DeleteAsync(model);
            return result;
        }

        public async Task<Result> DeleteByCodeAsync(StringModel model)
        {
            var result = await _efCommentRepository.DeleteByCodeAsync(model);
            return result;
        }

        public async Task<DataResult<IReadOnlyList<CommentModel>>> GetAllAsync()
        {
            var result = await _dapperCommentRepository.GetAllAsync();
            var resultData = _mapper.Map<DataResult<IReadOnlyList<CommentModel>>>(result);
            return resultData;
        }

        public async Task<DataResult<IReadOnlyList<CommentModel>>> GetAllByProductCode(IntModel model)
        {
            var result = await _dapperCommentRepository.GetAllByProductCode(model);
            var resultData = _mapper.Map<DataResult<IReadOnlyList<CommentModel>>>(result);
            return resultData;
        }

        public async Task<DataResult<IReadOnlyList<CommentModel>>> GetAllByProductId(IntModel model)
        {
            var result = await _dapperCommentRepository.GetAllByProductId(model);
            var resultData = _mapper.Map<DataResult<IReadOnlyList<CommentModel>>>(result);
            return resultData;
        }

        public async Task<DataResult<IReadOnlyList<CommentModel>>> GetAllByUserId(StringModel model)
        {
            var result = await _dapperCommentRepository.GetAllByUserId(model);
            var resultData = _mapper.Map<DataResult<IReadOnlyList<CommentModel>>>(result);
            return resultData;
        }

        public async Task<DataResult<IReadOnlyList<CommentModel>>> GetAllPagedAsync(PagingModel model)
        {
            var result = await _dapperCommentRepository.GetAllPagedAsync(model);
            var resultData = _mapper.Map<DataResult<IReadOnlyList<CommentModel>>>(result);
            return resultData;
        }

        public async Task<DataResult<CommentModel>> GetAsync(IntModel model)
        {
            var result = await _dapperCommentRepository.GetAsync(model);
            var resultData = _mapper.Map<DataResult<CommentModel>>(result);
            return resultData;
        }

        public async Task<DataResult<CommentModel>> GetByCodeAsync(StringModel model)
        {
            var result = await _dapperCommentRepository.GetByCodeAsync(model);
            var resultData = _mapper.Map<DataResult<CommentModel>>(result);
            return resultData;
        }
    }
}
