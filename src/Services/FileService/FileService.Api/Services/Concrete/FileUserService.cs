using AutoMapper;
using FileService.Api.Entities;
using FileService.Api.Infrastructure.Context;
using FileService.Api.Models.FileUserModels;
using FileService.Api.Services.Abstract;
using FileService.Api.Utilities.Results;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace FileService.Api.Services.Concrete;

public class FileUserService : IFileUserService
{
    private readonly FileDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<FileUserService> _logger;

    public FileUserService(
        FileDbContext dbContext, 
        IMapper mapper, 
        ILogger<FileUserService> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result> AddAsync(FileUserModel model)
    {
        _dbContext.Connection.Open();
        using (var transaction = _dbContext.Connection.BeginTransaction())
        {
            try
            {
                var data = _mapper.Map<FileUser>(model);

                _dbContext.Database.UseTransaction(transaction as DbTransaction);

                await _dbContext.FileUsers.AddAsync(data);

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

    public async Task<Result> UpdateAsync(FileUserModel model)
    {
        _dbContext.Connection.Open();
        using (var transaction = _dbContext.Connection.BeginTransaction())
        {
            try
            {
                _dbContext.Database.UseTransaction(transaction as DbTransaction);

                var result = await _dbContext.FileUsers.Where(c => c.Id == model.Id)
                                             .ExecuteUpdateAsync(c => c
                                             .SetProperty(p => p.Name, model.Name)
                                             .SetProperty(p => p.Description, model.Description));

                _dbContext.SaveChanges();
                if (result < 1)
                    return new ErrorResult("Error update");

                transaction.Commit();
                return new SuccessResult("Updated");
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

                var result = await _dbContext.FileUsers
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

    public async Task<DataResult<FileUserModel>> GetByIdAsync(int id)
    {
        var result = await _dbContext.FileUsers.FirstOrDefaultAsync(f => f.Id == id);
        var data = _mapper.Map<FileUserModel>(result);
        return new SuccessDataResult<FileUserModel>(data);
    }

    public async Task<DataResult<List<FileUserModel>>> GetByNameAsync(string name)
    {
        var list = await _dbContext.FileUsers
                                   .Where(f => f.Name.ToLower().Contains(name.ToLower()))
                                   .ToListAsync();

        var data = _mapper.Map<List<FileUserModel>>(list);
        return new SuccessDataResult<List<FileUserModel>>(data);
    }
}
