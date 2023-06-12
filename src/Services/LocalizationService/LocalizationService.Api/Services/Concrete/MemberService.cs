using AutoMapper;
using AutoMapper.Execution;
using LocalizationService.Api.Data.Repositories.Base;
using LocalizationService.Api.Entities;
using LocalizationService.Api.Models.Base.Concrete;
using LocalizationService.Api.Models.MemberModels;
using LocalizationService.Api.Services.Abstract;
using LocalizationService.Api.Services.Redis.Abstract;
using LocalizationService.Api.Utilities.Results;
using Polly;
using System.Reflection;

namespace LocalizationService.Api.Services.Concrete
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IRedisService _redisService;
        private readonly ILogger<MemberService> _logger;

        public MemberService(IUnitOfWork unitOfWork, 
                             IRedisService redisService,
                             IMapper mapper,
                             IConfiguration configuration,
                             ILogger<MemberService> logger)
        {
            _unitOfWork = unitOfWork;
            _redisService = redisService;
            _mapper = mapper;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<Result> AddAsync(MemberAddModel model)
        {
            var mappedMember = _mapper.Map<Entities.Member>(model);
            var result = await _unitOfWork.EfMemberRepository.AddAsync(mappedMember);

            return result;
        }

        public async Task<Result> UpdateAsync(MemberUpdateModel model)
        {
            var mappedMember = _mapper.Map<Entities.Member>(model);
            var result = await _unitOfWork.EfMemberRepository.UpdateAsync(mappedMember);

            return result;
        }

        public async Task<Result> DeleteAsync(StringModel model)
        {
            var result = await _unitOfWork.EfMemberRepository.DeleteAsync(model);

            return result;
        }

        public async Task<DataResult<IReadOnlyList<MemberModel>>> SaveToDbAsync(StringModel model)
        {
            var duration = _configuration.GetSection("LocalizationCacheSettings:Duration").Get<int>();
            var databaseId = _configuration.GetSection("LocalizationCacheSettings:DatabaseId").Get<int>();

            var anyExists = _redisService.AnyKeyExistsByPrefix(model.Value, databaseId);

            if (anyExists)
            {
                var cacheData = await _redisService.GetAsync<IReadOnlyList<MemberModel>>(model.Value);
                return new SuccessDataResult<IReadOnlyList<MemberModel>>(cacheData);
            }

            var memberResult = await _unitOfWork.MemberRepository.GetAllWithResourcesByMemberKeyAsync(model);

            if (memberResult.Success)
            {
                var data = memberResult.Data;

                var policy = Polly.Policy.Handle<Exception>()
                            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                            {
                                _logger.LogError(ex, "ERROR handling message: {ExceptionMessage} - Method : {ClassName}.{MethodName}",
                                                 ex.Message,
                                                 nameof(SaveToDbAsync),
                                                 MethodBase.GetCurrentMethod()?.Name);
                            });

                await policy.ExecuteAsync(async () =>
                {
                    await _redisService.SetAsync(model.Value, data, duration, databaseId);

                    var anyExists = _redisService.AnyKeyExistsByPrefix(model.Value, databaseId);

                    if (!anyExists)
                        throw new Exception($"Error redis cache saving : {model.Value}");
                });

                var result = _mapper.Map<IReadOnlyList<MemberModel>>(data);
                return new SuccessDataResult<IReadOnlyList<MemberModel>>(result);
            }
            else
            {
                return new ErrorDataResult<IReadOnlyList<MemberModel>>();
            }
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
