﻿using Dapper;
using LocalizationService.Api.Data.Contexts;
using LocalizationService.Api.Data.Contexts.Connections.Abstract;
using LocalizationService.Api.Data.Repositories.Dapper.Abstract;
using LocalizationService.Api.Entities;
using LocalizationService.Api.Extensions;
using LocalizationService.Api.Models.Base.Concrete;
using LocalizationService.Api.Utilities.Results;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Data.Common;

namespace LocalizationService.Api.Data.Repositories.Dapper.Concrete
{
    public class DapperLanguageRepository : IDapperLanguageRepository
    {
        private readonly ILocalizationDbContext _dbContext;
        private readonly ILocalizationReadDbConnection _readDbConnection;
        private readonly ILocalizationWriteDbConnection _writeDbConnection;

        private string _languageTable;
        private string _resourceTable;

        public DapperLanguageRepository(ILocalizationDbContext dbContext,
                                        ILocalizationReadDbConnection readDbConnection,
                                        ILocalizationWriteDbConnection writeDbConnection)
        {
            _dbContext = dbContext;
            _readDbConnection = readDbConnection;
            _writeDbConnection = writeDbConnection;

            _languageTable = _dbContext.GetTableNameWithScheme<Language>();
            _resourceTable = _dbContext.GetTableNameWithScheme<Resource>();
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

                //Add language, with SELECT CAST... we get the added language's id
                var addQuery = $"INSERT INTO {_languageTable}(Code,DisplayName) VALUES (@Code,@DisplayName);SELECT CAST(SCOPE_IDENTITY() as int)";
                var languageId = await _writeDbConnection.QuerySingleAsync<int>(sql: addQuery,
                                                                                transaction: transaction,
                                                                                param: new { Code = entity.Code, DisplayName = entity.DisplayName });
                if (languageId == 0)
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
                bool languageExists = await _dbContext.Languages.AnyAsync(l => l.Code == model.Value);
                if (!languageExists)
                    return new ErrorResult("Language does not exist");

                //Delete query
                var deleteQuery = $"DELETE FROM {_languageTable} WHERE Code=@Code";
                var result = await _writeDbConnection.ExecuteAsync(sql: deleteQuery,
                                                                   transaction: transaction,
                                                                   param: new { Code = model.Value });

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

        public async Task<Result> UpdateAsync(Language entity)
        {
            _dbContext.Connection.Open();
            using var transaction = _dbContext.Connection.BeginTransaction();
            try
            {
                var languageExists = await _dbContext.Languages.FirstOrDefaultAsync(l => l.Code == entity.Code);
                if (languageExists == null)
                    return new ErrorResult("Language does not exist");

                bool languageCodeExists = await _dbContext.Languages.AnyAsync(l => l.Id != languageExists.Id && l.Code == entity.Code);
                if (languageCodeExists)
                    return new ErrorResult("Language code already exists");

                //Update query
                var updateQuery = $"UPDATE {_languageTable} SET Code = @Code, DisplayName = @DisplayName WHERE Code=@ExistingCode";
                var result = await _writeDbConnection.ExecuteAsync(sql: updateQuery,
                                                                   transaction: transaction,
                                                                   param: new { Code = entity.Code, DisplayName = entity.DisplayName, ExistingCode = languageExists.Code });

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

        public async Task<DataResult<IReadOnlyList<Language>>> GetAllAsync()
        {
            var query = $"SELECT Id,Code,DisplayName FROM {_languageTable}";

            var result = await _readDbConnection.QueryAsync<Language>(query);
            return new DataResult<IReadOnlyList<Language>>(result);
        }

        public async Task<DataResult<IReadOnlyList<Language>>> GetAllWithResourcesAsync()
        {
            var query = $"SELECT l.Id, l.Code, l.DisplayName, r.Id as ResourceId, r.* " +
                        $"FROM {_languageTable} l " +
                        $"INNER JOIN {_resourceTable} r ON l.Id = r.LanguageId";

            var languageDictionary = new Dictionary<int, Language>();

            var result = await _dbContext.Connection.QueryAsync<Language, Resource, Language>(query, (language, resource) =>
            {
                Language languageEntry;

                if (!languageDictionary.TryGetValue(language.Id, out languageEntry))
                {
                    languageEntry = language;
                    languageEntry.Resources = languageEntry.Resources ?? new List<Resource>();
                    languageDictionary.Add(languageEntry.Id, languageEntry);
                }

                languageEntry.Resources.Add(resource);
                return languageEntry;
            }, splitOn: "ResourceId");

            var filteredResult = result.Distinct().ToList();
            return new DataResult<IReadOnlyList<Language>>(filteredResult);
        }

        public async Task<DataResult<Language>> GetAsync(StringModel model)
        {
            var query = $"SELECT Id,Code,DisplayName FROM {_languageTable} WHERE Code=@Code";

            var result = await _readDbConnection.QuerySingleAsync<Language>(query, new { Code = model.Value });
            return new DataResult<Language>(result);
        }
    }
}
