using AutoMapper;
using FileService.Api.Entities;
using FileService.Api.Infrastructure.Context;
using FileService.Api.Models.ImageModels;
using FileService.Api.Services.Abstract;
using FileService.Api.Utilities.Results;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace FileService.Api.Services.Concrete;

public class ImageService : IImageService
{
    private readonly IMapper _mapper;
    private readonly FileDbContext _dbContext;
    private readonly ILogger<ImageService> _logger;

    public ImageService(
        IMapper mapper,
        FileDbContext dbContext,
        ILogger<ImageService> logger)
    {
        _mapper = mapper;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result> AddAsync(ImageModel model)
    {
        _dbContext.Connection.Open();
        using (var transaction = _dbContext.Connection.BeginTransaction())
        {
            try
            {
                var data = _mapper.Map<Image>(model);

                _dbContext.Database.UseTransaction(transaction as DbTransaction);

                await _dbContext.Images.AddAsync(data);

                var result = _dbContext.SaveChanges();

                if (result < 1)
                    return new ErrorResult("Error add");

                transaction.Commit();
                return new SuccessResult("Added");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "ERROR - {Message}", ex.Message);
                throw new Exception(ex.Message);
            }
            finally
            {
                _dbContext.Connection.Close();
            }
        }

    }

    public async Task<Result> DeleteAsync(int id)
    {
        _dbContext.Connection.Open();
        using (var transaction = _dbContext.Connection.BeginTransaction())
        {
            try
            {
                _dbContext.Database.UseTransaction(transaction as DbTransaction);

                var result = await _dbContext.Images
                                             .Where(c => c.Id == id)
                                             .ExecuteDeleteAsync();

                _dbContext.SaveChanges();

                if (result < 1)
                    return new ErrorResult("Delete error"); ;

                transaction.Commit();
                return new SuccessResult("Deleted");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "ERROR - {Message}", ex.Message);
                throw new Exception(ex.Message);
            }
            finally
            {
                _dbContext.Connection.Close();
            }
        }
    }

    public async Task<DataResult<List<ImageModel>>> GetAllByTypeAndFileUserIdAsync(
        ImageTypeAndFileUserIdModel model)
    {
        var list = await _dbContext.Images
                                   .Where(i => i.FileUserId == model.FileUserId && i.Type == model.Type)
                                   .ToListAsync();

        var data = _mapper.Map<List<ImageModel>>(list);
        return new SuccessDataResult<List<ImageModel>>(data);
    }

    public async Task<DataResult<List<ImageModel>>> GetByTypeAndFileUserIdPagingAsync(
        ImageTypeAndFileUserIdPagingModel model)
    {
        var list = await _dbContext.Images
                                   .Where(i => i.FileUserId == model.FileUserId && i.Type == model.Type)
                                   .Skip((model.Page - 1) * model.PageSize)
                                   .Take(model.PageSize)
                                   .ToListAsync();

        var data = _mapper.Map<List<ImageModel>>(list);
        return new SuccessDataResult<List<ImageModel>>(data);
    }

    public async Task<DataResult<List<ImageModel>>> GetAllByTypeAsync(
        ImageTypeModel model)
    {
        var list = await _dbContext.Images
                                   .Where(i => i.Type == model.Type)
                                   .ToListAsync();

        var data = _mapper.Map<List<ImageModel>>(list);
        return new SuccessDataResult<List<ImageModel>>(data);
    }

    public async Task<DataResult<List<ImageModel>>> GetAllByTypePagingAsync(
        ImageTypePagingModel model)
    {
        var list = await _dbContext.Images
                                   .Where(i => i.Type == model.Type)
                                   .Skip((model.Page - 1) * model.PageSize)
                                   .Take(model.PageSize)
                                   .ToListAsync();

        var data = _mapper.Map<List<ImageModel>>(list);
        return new SuccessDataResult<List<ImageModel>>(data);
    }

    public async Task<DataResult<ImageModel>> GetByIdAsync(int id)
    {
        var data = await _dbContext.Images.FirstOrDefaultAsync(i => i.Id == id);

        var result = _mapper.Map<ImageModel>(data);
        return new SuccessDataResult<ImageModel>(result);
    }
}
