using AutoMapper;
using LocalizationService.Api.Data.Repositories.Base;
using LocalizationService.Api.Models.Base.Concrete;
using LocalizationService.Api.Models.IncludeOptions;
using LocalizationService.Api.Models.LanguageModels;
using LocalizationService.Api.Services.Abstract;
using LocalizationService.Api.Utilities.Results;

namespace LocalizationService.Api.Services.Concrete
{
    public class LanguageService : ILanguageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LanguageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> AddAsync(LanguageAddModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> UpdateAsync(LanguageUpdateModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> DeleteAsync(StringModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResult<IReadOnlyList<LanguageModel>>> GetAllAsync()
        {
            var languages = await _unitOfWork.LanguageRepository.GetAllAsync();
            var result = _mapper.Map<IReadOnlyList<LanguageModel>>(languages);

            return new DataResult<IReadOnlyList<LanguageModel>>(result);
        }

        public async Task<DataResult<IReadOnlyList<LanguageModel>>> GetAllWithResourcesAsync()
        {
            var languages = await _unitOfWork.LanguageRepository.GetAllWithResourcesAsync();
            var result = _mapper.Map<IReadOnlyList<LanguageModel>>(languages.Data);

            return new DataResult<IReadOnlyList<LanguageModel>>(result);
        }

        public async Task<DataResult<LanguageModel>> GetAsync(StringModel model)
        {
            var language = await _unitOfWork.LanguageRepository.GetAsync(model);
            var result = _mapper.Map<LanguageModel>(language.Data);

            return new DataResult<LanguageModel>(result);
        }

        public async Task<DataResult<IReadOnlyList<LanguageModel>>> GetAllWithResourcesPagingAsync(PagingModel model)
        {
            var languages = await _unitOfWork.LanguageRepository.GetAllWithResourcesPagingAsync(model);
            var result = _mapper.Map<IReadOnlyList<LanguageModel>>(languages.Data);

            return new DataResult<IReadOnlyList<LanguageModel>>(result);
        }

        public async Task<DataResult<IReadOnlyList<LanguageModel>>> GetAllPagingAsync(PagingModel model)
        {
            var languages = await _unitOfWork.LanguageRepository.GetAllPagingAsync(model);
            var result = _mapper.Map<IReadOnlyList<LanguageModel>>(languages.Data);

            return new DataResult<IReadOnlyList<LanguageModel>>(result);
        }
    }
}
