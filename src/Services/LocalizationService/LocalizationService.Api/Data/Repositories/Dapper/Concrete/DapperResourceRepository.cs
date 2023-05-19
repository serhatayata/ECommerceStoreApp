using Dapper;
using LocalizationService.Api.Data.Contexts;
using LocalizationService.Api.Data.Contexts.Connections.Abstract;
using LocalizationService.Api.Data.Repositories.Dapper.Abstract;
using LocalizationService.Api.Entities;
using LocalizationService.Api.Models.Base.Concrete;
using LocalizationService.Api.Utilities.Results;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace LocalizationService.Api.Data.Repositories.Dapper.Concrete
{
    public class DapperResourceRepository : IDapperResourceRepository
    {
        private readonly ILocalizationDbContext _dbContext;
        private readonly ILocalizationReadDbConnection _readDbConnection;
        private readonly ILocalizationWriteDbConnection _writeDbConnection;

        public DapperResourceRepository(ILocalizationDbContext dbContext, 
                                        ILocalizationReadDbConnection readDbConnection, 
                                        ILocalizationWriteDbConnection writeDbConnection)
        {
            _dbContext = dbContext;
            _readDbConnection = readDbConnection;
            _writeDbConnection = writeDbConnection;
        }

        public async Task<Result> AddAsync(Resource entity)
        {
            _dbContext.Connection.Open();
            using var transaction = _dbContext.Connection.BeginTransaction();
            try
            {
                _dbContext.Database.UseTransaction(transaction as DbTransaction);
                //Check if resource exists
                bool resourceExists = await _dbContext.Resources.AnyAsync(l => l.ResourceCode == entity.ResourceCode || 
                                                                               (l.Tag == entity.Tag && l.MemberId == entity.MemberId));

                var existingLanguage = await _dbContext.Languages.FirstOrDefaultAsync(l => l.Code == entity.LanguageCode);

                if (resourceExists)
                    return new ErrorResult("Resource already exists");

                //Add language, with SELECT CAST... we get the added language's id
                var addQuery = "INSERT INTO Resources(LanguageId,MemberId,Tag,Value,ResourceCode,LanguageCode,Status) " +
                               "VALUES (@LanguageId,@MemberId,@Tag,@Value,@ResourceCode,@LanguageCode,@Status);" +
                               "SELECT CAST(SCOPE_IDENTITY() as int)";

                var resourceId = await _writeDbConnection.QuerySingleAsync<int>(sql: addQuery,
                                                                                transaction: transaction,
                                                                                param: new { LanguageId = existingLanguage?.Id });
                if (resourceId == 0)
                    return new ErrorResult("Resource not added");

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
                bool resourceExists = await _dbContext.Resources.AnyAsync(l => l.ResourceCode == model.Value);
                if (!resourceExists)
                    return new ErrorResult("Resource does not exist");

                //Delete query
                var deleteQuery = "DELETE FROM Resources WHERE ResourceCode=@ResourceCode";
                var result = await _writeDbConnection.ExecuteAsync(sql: deleteQuery,
                                                                   transaction: transaction,
                                                                   param: new { ResourceCode = model.Value });

                transaction.Commit();

                return result > 0 ? new SuccessResult() : new ErrorResult("Resource not added");
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
                var resourceExists = await _dbContext.Resources.FirstOrDefaultAsync(l => l.ResourceCode == entity.ResourceCode);
                if (resourceExists == null)
                    return new ErrorResult("Resource does not exist");

                bool resourceTagExists = await _dbContext.Resources.AnyAsync(l => l.Id != resourceExists.Id &&
                                                                                  l.LanguageCode == resourceExists.LanguageCode &&
                                                                                  l.MemberId == resourceExists.MemberId &&
                                                                                  l.Tag == entity.Tag);

                if (resourceTagExists)
                    return new ErrorResult("Resource code already exists");

                //Update query
                var updateQuery = "UPDATE Resources SET Tag = @Tag, Value = @Value, Status = @Status WHERE ResourceCode = @ResourceCode";
                var result = await _writeDbConnection.ExecuteAsync(sql: updateQuery,
                                                                   transaction: transaction,
                                                                   param: new { Tag = entity.Tag, 
                                                                                Value = entity.Value, 
                                                                                Status = entity.Status, 
                                                                                ResourceCode = entity.ResourceCode });

                transaction.Commit();

                return result > 0 ? new SuccessResult() : new ErrorResult("Language not added");
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
            var query = "SELECT r.Id, r.LanguageId, r.MemberId, r.Tag, r.Value, r.ResourceCode, r.LanguageCode, r.CreateDate, r.Status " +
                        "FROM Resources r " +
                        "INNER JOIN Languages l ON l.Id = r.LanguageId " +
                        "INNER JOIN Members m ON m.Id = r.MemberId";

            var result = await _dbContext.Connection.QueryAsync<Resource,Language,Member,Resource>(query, (resource,language,member) =>
            {
                resource.Language = language;
                resource.Member = member;
                return resource;
            }, splitOn: "Id");

            return new DataResult<IReadOnlyList<Resource>>(result.Distinct().ToList());
        }

        public async Task<DataResult<IReadOnlyList<Resource>>> GetAllActiveAsync()
        {
            var query = "SELECT r.Id, r.LanguageId, r.MemberId, r.Tag, r.Value, r.ResourceCode, r.LanguageCode, r.CreateDate, r.Status " +
                        "FROM Resources r " +
                        "INNER JOIN Languages l ON l.Id = r.LanguageId " +
                        "INNER JOIN Members m ON m.Id = r.MemberId " +
                        "WHERE Status = @Status";

            var result = await _dbContext.Connection.QueryAsync<Resource, Language, Member, Resource>(query, (resource, language, member) =>
            {
                resource.Language = language;
                resource.Member = member;
                return resource;
            }, splitOn: "Id", param : new { Status = 1 });

            return new DataResult<IReadOnlyList<Resource>>(result.Distinct().ToList());
        }

        public async Task<DataResult<Resource>> GetAsync(StringModel model)
        {
            var query = "SELECT Id, LanguageId, MemberId, Tag, Value, ResourceCode, LanguageCode, CreateDate, Status FROM Resources WHERE ResourceCode = @ResourceCode";

            var result = await _readDbConnection.QuerySingleAsync<Resource>(query, new { ResourceCode = model.Value });
            return new DataResult<Resource>(result);
        }
    }
}
