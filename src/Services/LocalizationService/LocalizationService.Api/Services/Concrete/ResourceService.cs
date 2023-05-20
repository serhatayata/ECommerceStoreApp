using AutoMapper;
using LocalizationService.Api.Data.Repositories.Base;
using LocalizationService.Api.Models.Base.Concrete;
using LocalizationService.Api.Models.MemberModels;
using LocalizationService.Api.Models.ResourceModels;
using LocalizationService.Api.Services.Abstract;
using LocalizationService.Api.Utilities.Results;

namespace LocalizationService.Api.Services.Concrete
{
    public class ResourceService : IResourceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ResourceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> AddAsync(ResourceAddModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> UpdateAsync(ResourceUpdateModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> DeleteAsync(StringModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResult<IReadOnlyList<ResourceModel>>> GetAllAsync()
        {
            var resources = await _unitOfWork.ResourceRepository.GetAllAsync();
            var result = _mapper.Map<IReadOnlyList<ResourceModel>>(resources.Data);

            return new DataResult<IReadOnlyList<ResourceModel>>(result);
        }

        public async Task<DataResult<IReadOnlyList<ResourceModel>>> GetAllPagingAsync(PagingModel model)
        {
            var resources = await _unitOfWork.ResourceRepository.GetAllPagingAsync(model);
            var result = _mapper.Map<IReadOnlyList<ResourceModel>>(resources.Data);

            return new DataResult<IReadOnlyList<ResourceModel>>(result);
        }

        public async Task<DataResult<IReadOnlyList<ResourceModel>>> GetAllActiveAsync()
        {
            var resources = await _unitOfWork.ResourceRepository.GetAllActiveAsync();
            var result = _mapper.Map<IReadOnlyList<ResourceModel>>(resources.Data);

            return new DataResult<IReadOnlyList<ResourceModel>>(result);
        }

        public async Task<DataResult<IReadOnlyList<ResourceModel>>> GetAllActivePagingAsync(PagingModel model)
        {
            var resources = await _unitOfWork.ResourceRepository.GetAllActivePagingAsync(model);
            var result = _mapper.Map<IReadOnlyList<ResourceModel>>(resources.Data);

            return new DataResult<IReadOnlyList<ResourceModel>>(result);
        }

        public async Task<DataResult<ResourceModel>> GetAsync(StringModel model)
        {
            var resource = await _unitOfWork.ResourceRepository.GetAsync(model);
            var result = _mapper.Map<ResourceModel>(resource);

            return new DataResult<ResourceModel>(result);

        }
    }
}
