using AutoMapper;
using LocalizationService.Api.Data.Repositories.Base;
using LocalizationService.Api.Entities;
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
            var mappedResource = _mapper.Map<Resource>(model);

            var currentMember = await _unitOfWork.MemberRepository.GetAsync(new StringModel() { Value = model.MemberKey });
            if (currentMember == null)
                return new ErrorResult("Member not found");

            mappedResource.MemberId = currentMember.Data.Id;
            var result = await _unitOfWork.EfResourceRepository.AddAsync(mappedResource);

            return result;
        }

        public async Task<Result> UpdateAsync(ResourceUpdateModel model)
        {
            var mappedResource = _mapper.Map<Resource>(model);
            var result = await _unitOfWork.EfResourceRepository.UpdateAsync(mappedResource);

            return result;
        }

        public async Task<Result> DeleteAsync(StringModel model)
        {
            var result = await _unitOfWork.EfResourceRepository.DeleteAsync(model);

            return result;
        }

        public async Task<DataResult<IReadOnlyList<ResourceModel>>> GetAllAsync()
        {
            var resources = await _unitOfWork.ResourceRepository.GetAllAsync();
            var result = _mapper.Map<IReadOnlyList<ResourceModel>>(resources.Data);

            return new SuccessDataResult<IReadOnlyList<ResourceModel>>(result);
        }

        public async Task<DataResult<IReadOnlyList<ResourceModel>>> GetAllPagingAsync(PagingModel model)
        {
            var resources = await _unitOfWork.ResourceRepository.GetAllPagingAsync(model);
            var result = _mapper.Map<IReadOnlyList<ResourceModel>>(resources.Data);

            return new SuccessDataResult<IReadOnlyList<ResourceModel>>(result);
        }

        public async Task<DataResult<IReadOnlyList<ResourceModel>>> GetAllActiveAsync()
        {
            var resources = await _unitOfWork.ResourceRepository.GetAllActiveAsync();
            var result = _mapper.Map<IReadOnlyList<ResourceModel>>(resources.Data);

            return new SuccessDataResult<IReadOnlyList<ResourceModel>>(result);
        }

        public async Task<DataResult<IReadOnlyList<ResourceModel>>> GetAllActivePagingAsync(PagingModel model)
        {
            var resources = await _unitOfWork.ResourceRepository.GetAllActivePagingAsync(model);
            var result = _mapper.Map<IReadOnlyList<ResourceModel>>(resources.Data);

            return new SuccessDataResult<IReadOnlyList<ResourceModel>>(result);
        }

        public async Task<DataResult<ResourceModel>> GetAsync(StringModel model)
        {
            var resource = await _unitOfWork.ResourceRepository.GetAsync(model);
            var result = _mapper.Map<ResourceModel>(resource);

            if (result == null)
                return new ErrorDataResult<ResourceModel>();

            return new SuccessDataResult<ResourceModel>(result);

        }
    }
}
