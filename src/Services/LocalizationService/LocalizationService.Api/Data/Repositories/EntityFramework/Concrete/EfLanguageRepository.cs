using AutoMapper;
using LocalizationService.Api.Data.Contexts;
using LocalizationService.Api.Data.Repositories.EntityFramework.Abstract;
using LocalizationService.Api.Entities;
using LocalizationService.Api.Models.Base.Concrete;
using LocalizationService.Api.Utilities.Results;
using Microsoft.EntityFrameworkCore;
using Polly;
using System.Data.Common;

namespace LocalizationService.Api.Data.Repositories.EntityFramework.Concrete
{
    public class EfLanguageRepository : IEfLanguageRepository
    {
        private readonly ILocalizationDbContext _dbContext;
        private readonly IMapper _mapper;

        public EfLanguageRepository(ILocalizationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Result> AddAsync(Language entity)
        {
            _dbContext.Connection.Open();
            using var transaction = _dbContext.Connection.BeginTransaction();
            try
            {
                _dbContext.Database.UseTransaction(transaction as DbTransaction);
                //Check if language exists
                bool languageExists = await _dbContext.Languages.AnyAsync(l => l.Code == entity.Code);
                if (languageExists)
                    return new ErrorResult("Language code already exists");

                var addedLangugage = await _dbContext.Languages.AddAsync(entity);
                var addResult = await _dbContext.SaveChangesAsync(default);

                if (addResult < 1)
                    return new ErrorResult("Language not added");

                transaction.Commit();
                return new SuccessResult();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                _dbContext.Connection.Close();
            }
        }

        public async Task<Result> DeleteAsync(StringModel model)
        {
            _dbContext.Connection.Open();
            using var transaction = _dbContext.Connection.BeginTransaction();
            try
            {
                _dbContext.Database.UseTransaction(transaction as DbTransaction);

                var languageExists = await _dbContext.Languages.FirstOrDefaultAsync(l => l.Code == model.Value);
                if (languageExists == null)
                    return new ErrorResult("Language does not exist");

                var deletedLangugage = _dbContext.Languages.Remove(languageExists);
                var deleteResult = await _dbContext.SaveChangesAsync(default);

                if (deleteResult < 1)
                    return new ErrorResult("Language not added");

                transaction.Commit();

                return deleteResult > 0 ? new SuccessResult() : new ErrorResult("Language not added");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                _dbContext.Connection.Close();
            }
        }

        public async Task<Result> UpdateAsync(Language entity)
        {
            _dbContext.Connection.Open();
            using var transaction = _dbContext.Connection.BeginTransaction();
            try
            {
                _dbContext.Database.UseTransaction(transaction as DbTransaction);

                var languageExists = await _dbContext.Languages.FirstOrDefaultAsync(l => l.Code == entity.Code);
                if (languageExists == null)
                    return new ErrorResult("Language does not exist");

                bool languageCodeExists = await _dbContext.Languages.AnyAsync(l => l.Id != languageExists.Id && l.Code == entity.Code);
                if (languageCodeExists)
                    return new ErrorResult("Language code already exists");

                languageExists.DisplayName = entity.DisplayName;

                var updatedLanguage = _dbContext.Languages.Update(languageExists);
                var updateResult = await _dbContext.SaveChangesAsync(default);

                if (updateResult < 1)
                    return new ErrorResult("Language not updated");

                transaction.Commit();

                return updateResult > 0 ? new SuccessResult() : new ErrorResult("Language not added");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                _dbContext.Connection.Close();
            }
        }

        public async Task<DataResult<IReadOnlyList<Language>>> GetAllAsync()
        {
            var result = await _dbContext.Languages.ToListAsync();
            return new DataResult<IReadOnlyList<Language>>(result);
        }

        public async Task<DataResult<Language>> GetAsync(StringModel model)
        {
            var result = await _dbContext.Languages.FirstOrDefaultAsync();
            return new DataResult<Language>(result);
        }
    }
}
