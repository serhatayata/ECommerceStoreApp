using LocalizationService.Api.Data.Contexts;
using LocalizationService.Api.Data.Repositories.EntityFramework.Abstract;
using LocalizationService.Api.Entities;
using LocalizationService.Api.Models.Base.Concrete;
using LocalizationService.Api.Utilities.Results;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace LocalizationService.Api.Data.Repositories.EntityFramework.Concrete
{
    public class EfResourceRepository : IEfResourceRepository
    {
        private readonly ILocalizationDbContext _dbContext;

        public EfResourceRepository(ILocalizationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> AddAsync(Resource entity)
        {
            _dbContext.Connection.Open();
            using var transaction = _dbContext.Connection.BeginTransaction();
            try
            {
                _dbContext.Database.UseTransaction(transaction as DbTransaction);

                var existingLanguage = await _dbContext.Languages.FirstOrDefaultAsync(l => l.Code == entity.LanguageCode);
                if (existingLanguage == null)
                    return new ErrorResult("Language does not exist");

                //Check if resource exists
                bool resourceExists = await _dbContext.Resources.AnyAsync(l => l.ResourceCode == entity.ResourceCode ||
                                                                               (l.Tag == entity.Tag && l.MemberId == entity.MemberId));

                if (resourceExists)
                    return new ErrorResult("Resource already exists");

                entity.ResourceCode = Guid.NewGuid().ToString();
                entity.LanguageId = existingLanguage.Id;
                entity.Status = true;

                var addedResource = await _dbContext.Resources.AddAsync(entity);
                var addResult = await _dbContext.SaveChangesAsync(default);

                if (addResult < 1)
                    return new ErrorResult("Resource not added");

                transaction.Commit();
                return addResult > 0 ? new SuccessResult() : new ErrorResult("Resource not added");
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

                var resourceExists = await _dbContext.Resources.FirstOrDefaultAsync(l => l.ResourceCode == model.Value);
                if (resourceExists == null)
                    return new ErrorResult("Resource does not exist");

                var deleteResource = _dbContext.Resources.Remove(resourceExists);
                var deleteResult = await _dbContext.SaveChangesAsync(default);

                if (deleteResult < 1)
                    return new ErrorResult("Resource not added");

                transaction.Commit();

                return deleteResult > 0 ? new SuccessResult() : new ErrorResult("Resource not added");
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

        public async Task<Result> UpdateAsync(Resource entity)
        {
            _dbContext.Connection.Open();
            using var transaction = _dbContext.Connection.BeginTransaction();
            try
            {
                _dbContext.Database.UseTransaction(transaction as DbTransaction);

                var resourceExists = await _dbContext.Resources.FirstOrDefaultAsync(l => l.ResourceCode == entity.ResourceCode);
                if (resourceExists == null)
                    return new ErrorResult("Resource does not exist");

                bool resourceTagExists = await _dbContext.Resources.AnyAsync(l => l.Id != resourceExists.Id &&
                                                                                  l.LanguageCode == resourceExists.LanguageCode &&
                                                                                  l.MemberId == resourceExists.MemberId &&
                                                                                  l.Tag == entity.Tag);

                if (resourceTagExists)
                    return new ErrorResult("Resource code already exists");

                resourceExists.Tag = entity.Tag;
                resourceExists.Value = entity.Value;

                var updateResource = _dbContext.Resources.Update(resourceExists);
                var updateResult = await _dbContext.SaveChangesAsync(default);

                if (updateResult < 1)
                    return new ErrorResult("Resource not added");

                transaction.Commit();

                return updateResult > 0 ? new SuccessResult() : new ErrorResult("Resource not added");
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

        public async Task<DataResult<IReadOnlyList<Resource>>> GetAllAsync()
        {
            var result = await _dbContext.Resources.ToListAsync();
            return new DataResult<IReadOnlyList<Resource>>(result);
        }

        public async Task<DataResult<Resource>> GetAsync(StringModel model)
        {
            var result = await _dbContext.Resources.FirstOrDefaultAsync(r => r.ResourceCode == model.Value);
            return new DataResult<Resource>(result);
        }
    }
}
