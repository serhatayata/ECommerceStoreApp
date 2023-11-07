using AutoMapper;
using OrderService.Api.Entities;
using OrderService.Api.Extensions;
using OrderService.Api.Models.Base;
using OrderService.Api.Models.OrderModels;
using OrderService.Api.Repositories.EntityFramework.Abstract;
using OrderService.Api.Services.Abstract;
using OrderService.Api.Utilities.Results;

namespace OrderService.Api.Services.Concrete;

public class OrderService : IOrderService
{
    private readonly IEfOrderRepository _efOrderRepository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    private int codeLength;

    public OrderService(
        IEfOrderRepository efOrderRepository,
        IMapper mapper,
        IConfiguration configuration)
    {
        _efOrderRepository = efOrderRepository;
        _mapper = mapper;
        _configuration = configuration;

        codeLength = _configuration.GetValue<int>("OrderCodeGenerationLength");
    }

    public async Task<DataResult<int>> AddAsync(OrderAddModel model)
    {
        var mappedModel = _mapper.Map<Order>(model);
        //Code generation
        var code = DataGenerationExtensions.RandomCode(codeLength);
        //Code exists
        var orderCodeExists = await _efOrderRepository.GetAsync(c => c.Code == code);
        if (orderCodeExists.Success)
            return new ErrorDataResult<int>(default(int), "order already exists");

        mappedModel.Code = code;
        var result = await _efOrderRepository.AddAsync(mappedModel);
        return result;
    }

    public async Task<Result> UpdateAsync(OrderUpdateModel model)
    {
        var mappedModel = _mapper.Map<Order>(model);
        //Check if name changed
        var existingOrder = await _efOrderRepository.GetAsync(new IntModel(model.Id));
        if (existingOrder?.Data != null)
        {
            var code = DataGenerationExtensions.RandomCode(codeLength);
            mappedModel.Code = code;
        }

        var result = await _efOrderRepository.UpdateAsync(mappedModel);
        return result;
    }

    public async Task<Result> DeleteAsync(IntModel model)
    {
        var result = await _efOrderRepository.DeleteAsync(model);
        return result;
    }

    public async Task<DataResult<IReadOnlyList<OrderModel>>> GetAllAsync()
    {
        var result = await _efOrderRepository.GetAllAsync();
        var resultData = _mapper.Map<DataResult<IReadOnlyList<OrderModel>>>(result);

        return resultData;
    }

    public async Task<DataResult<OrderModel>> GetAsync(IntModel model)
    {
        var result = await _efOrderRepository.GetAsync(model);
        var resultData = _mapper.Map<DataResult<OrderModel>>(result);

        return resultData;
    }
}
