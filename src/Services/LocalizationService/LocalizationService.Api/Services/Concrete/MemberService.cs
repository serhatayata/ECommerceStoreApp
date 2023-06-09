using AutoMapper;
using LocalizationService.Api.Data.Repositories.Base;
using LocalizationService.Api.Entities;
using LocalizationService.Api.Models.Base.Concrete;
using LocalizationService.Api.Models.MemberModels;
using LocalizationService.Api.Services.Abstract;
using LocalizationService.Api.Utilities.Results;

namespace LocalizationService.Api.Services.Concrete
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> AddAsync(MemberAddModel model)
        {
            var mappedMember = _mapper.Map<Member>(model);
            var result = await _unitOfWork.EfMemberRepository.AddAsync(mappedMember);

            return result;
        }

        public async Task<Result> UpdateAsync(MemberUpdateModel model)
        {
            var mappedMember = _mapper.Map<Member>(model);
            var result = await _unitOfWork.EfMemberRepository.UpdateAsync(mappedMember);

            return result;
        }

        public async Task<Result> DeleteAsync(StringModel model)
        {
            var result = await _unitOfWork.EfMemberRepository.DeleteAsync(model);

            return result;
        }

        public async Task<DataResult<IReadOnlyList<MemberModel>>> GetAllAsync()
        {
            var members = await _unitOfWork.MemberRepository.GetAllAsync();
            var result = _mapper.Map<IReadOnlyList<MemberModel>>(members.Data);

            return new SuccessDataResult<IReadOnlyList<MemberModel>>(result);
        }

        public async Task<DataResult<IReadOnlyList<MemberModel>>> GetAllPagingAsync(PagingModel model)
        {
            var members = await _unitOfWork.MemberRepository.GetAllPagingAsync(model);
            var result = _mapper.Map<IReadOnlyList<MemberModel>>(members.Data);

            return new SuccessDataResult<IReadOnlyList<MemberModel>>(result);
        }

        public async Task<DataResult<IReadOnlyList<MemberModel>>> GetAllWithResourcesAsync()
        {
            var members = await _unitOfWork.MemberRepository.GetAllWithResourcesAsync();
            var result = _mapper.Map<IReadOnlyList<MemberModel>>(members.Data);

            return new SuccessDataResult<IReadOnlyList<MemberModel>>(result);
        }

        public async Task<DataResult<IReadOnlyList<MemberModel>>> GetAllWithResourcesPagingAsync(PagingModel model)
        {
            var members = await _unitOfWork.MemberRepository.GetAllWithResourcesPagingAsync(model);
            var result = _mapper.Map<IReadOnlyList<MemberModel>>(members.Data);

            return new SuccessDataResult<IReadOnlyList<MemberModel>>(result);
        }

        public async Task<DataResult<IReadOnlyList<MemberModel>>> GetAllWithResourcesByMemberKeyAsync(StringModel model)
        {
            var members = await _unitOfWork.MemberRepository.GetAllWithResourcesByMemberKeyAsync(model);
            var result = _mapper.Map<IReadOnlyList<MemberModel>>(members.Data);

            return new SuccessDataResult<IReadOnlyList<MemberModel>>(result);
        }

        public async Task<DataResult<MemberModel>> GetAsync(StringModel model)
        {
            var member = await _unitOfWork.MemberRepository.GetAsync(model);
            var result = _mapper.Map<MemberModel>(member.Data);

            if (result == null)
                return new ErrorDataResult<MemberModel>();

            return new SuccessDataResult<MemberModel>(result);

        }
    }
}
