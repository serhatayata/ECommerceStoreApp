using LocalizationService.Api.Data.Contexts.Connections.Abstract;
using LocalizationService.Api.Data.Contexts;
using LocalizationService.Api.Data.Repositories.Dapper.Abstract;
using LocalizationService.Api.Entities;
using LocalizationService.Api.Models.Base.Concrete;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Dapper;
using LocalizationService.Api.Utilities.Results;

namespace LocalizationService.Api.Data.Repositories.Dapper.Concrete
{
    public class DapperMemberRepository : IDapperMemberRepository
    {
        private readonly ILocalizationDbContext _dbContext;
        private readonly ILocalizationReadDbConnection _readDbConnection;
        private readonly ILocalizationWriteDbConnection _writeDbConnection;

        private readonly string _memberTable;
        private readonly string _resourceTable;

        public DapperMemberRepository(ILocalizationDbContext dbContext,
                                      ILocalizationReadDbConnection readDbConnection,
                                      ILocalizationWriteDbConnection writeDbConnection)
        {
            _dbContext = dbContext;
            _readDbConnection = readDbConnection;
            _writeDbConnection = writeDbConnection;

            _memberTable = dbContext.GetTableNameWithScheme<Member>();
            _resourceTable = dbContext.GetTableNameWithScheme<Resource>();
        }

        public async Task<Result> AddAsync(Member entity)
        {
            _dbContext.Connection.Open();
            using var transaction = _dbContext.Connection.BeginTransaction();
            try
            {
                _dbContext.Database.UseTransaction(transaction as DbTransaction);
                //Check if member exists
                bool memberExists = await _dbContext.Members.AnyAsync(l => l.Name == entity.Name || l.MemberKey == entity.MemberKey);
                if (memberExists)
                    return new ErrorResult("Member already exists");

                //Add member, with SELECT CAST... we get the added member's id
                var addQuery = $"INSERT INTO {_memberTable}(Name,MemberKey) VALUES (@Name,@MemberKey);SELECT CAST(SCOPE_IDENTITY() as int)";
                var memberId = await _writeDbConnection.QuerySingleOrDefaultAsync<int>(sql: addQuery,
                                                                              transaction: transaction,
                                                                              param: new { Name = entity.Name, MemberKey = entity.MemberKey });
                if (memberId == 0)
                    return new ErrorResult("Member not added");

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
                bool memberExists = await _dbContext.Members.AnyAsync(l => l.MemberKey == model.Value);
                if (!memberExists)
                    return new ErrorResult("Member does not exist");

                //Delete query
                var deleteQuery = $"DELETE FROM {_memberTable} WHERE MemberKey=@MemberKey";
                var result = await _writeDbConnection.ExecuteAsync(sql: deleteQuery,
                                                                   transaction: transaction,
                                                                   param: new { MemberKey = model.Value });

                transaction.Commit();

                return result > 0 ? new SuccessResult() : new ErrorResult("Member not added");
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

        public async Task<Result> UpdateAsync(Member entity)
        {
            _dbContext.Connection.Open();
            using var transaction = _dbContext.Connection.BeginTransaction();
            try
            {
                var memberExists = await _dbContext.Members.FirstOrDefaultAsync(l => l.MemberKey == entity.MemberKey);
                if (memberExists == null)
                    return new ErrorResult("Member does not exist");

                if (memberExists.Name == entity.Name)
                    return new SuccessResult();

                bool memberNameExists = await _dbContext.Members.AnyAsync(l => l.Id != memberExists.Id && l.Name == entity.Name);
                if (memberNameExists)
                    return new ErrorResult("Member name already exists");

                //Update query
                var updateQuery = $"UPDATE {_memberTable} SET Name = @Name WHERE MemberKey=@MemberKey";
                var result = await _writeDbConnection.ExecuteAsync(sql: updateQuery,
                                                                   transaction: transaction,
                                                                   param: new { Name = entity.Name, MemberKey = entity.MemberKey });

                transaction.Commit();

                return result > 0 ? new SuccessResult() : new ErrorResult("Member not added");
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

        public async Task<DataResult<IReadOnlyList<Member>>> GetAllAsync()
        {
            var query = $"SELECT Id,Name,MemberKey,CreateDate FROM {_memberTable}";

            var result = await _readDbConnection.QueryAsync<Member>(query);
            return new DataResult<IReadOnlyList<Member>>(result);
        }

        public async Task<DataResult<IReadOnlyList<Member>>> GetAllPagingAsync(PagingModel model)
        {
            var query = $"SELECT Id,Name,MemberKey,CreateDate FROM {_memberTable} " +
                        $"ORDER BY Id DESC OFFSET (@Page-1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY";

            var result = await _readDbConnection.QueryAsync<Member>(sql: query, param: new { Page = model.Page, PageSize = model.PageSize });
            return new DataResult<IReadOnlyList<Member>>(result);
        }

        public async Task<DataResult<IReadOnlyList<Member>>> GetAllWithResourcesAsync()
        {
            var query = $"SELECT l.*, r.Id as ResourceId, r.* FROM {_memberTable} l " +
                        $"INNER JOIN {_resourceTable} r ON l.Id = r.MemberId";

            var memberDictionary = new Dictionary<int, Member>();

            var result = await _dbContext.Connection.QueryAsync<Member, Resource, Member>(query, (member, resource) =>
            {
                Member? memberEntry;

                if (!memberDictionary.TryGetValue(member.Id, out memberEntry))
                {
                    memberEntry = member;
                    memberEntry.Resources = new List<Resource>();
                    memberDictionary.Add(memberEntry.Id, memberEntry);
                }
                if (resource != null)
                    memberEntry.Resources.Add(resource);

                return memberEntry;
            }, splitOn: "ResourceId");

            var filteredResult = result.DistinctBy(m => m.Id).ToList();
            return new DataResult<IReadOnlyList<Member>>(filteredResult);
        }

        public async Task<DataResult<IReadOnlyList<Member>>> GetAllWithResourcesPagingAsync(PagingModel model)
        {
            var query = $"SELECT m.*, r.Id as ResourceId, r.* FROM " +
                        $"(SELECT * FROM {_memberTable} ORDER BY Id DESC OFFSET (@Page-1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY) m " +
                        $"INNER JOIN {_resourceTable} r ON m.Id = r.MemberId";

            var memberDictionary = new Dictionary<int, Member>();

            var result = await _dbContext.Connection.QueryAsync<Member, Resource, Member>(query, (member, resource) =>
            {
                Member? memberEntry;

                if (!memberDictionary.TryGetValue(member.Id, out memberEntry))
                {
                    memberEntry = member;
                    memberEntry.Resources = new List<Resource>();
                    memberDictionary.Add(memberEntry.Id, memberEntry);
                }
                if (resource != null)
                    memberEntry.Resources.Add(resource);

                return memberEntry;
            }, param: new { Page = model.Page, PageSize = model.PageSize }, splitOn: "ResourceId");

            var filteredResult = result.DistinctBy(m => m.Id).ToList();
            return new DataResult<IReadOnlyList<Member>>(filteredResult);
        }

        public async Task<DataResult<Member>> GetAsync(StringModel model)
        {
            var query = $"SELECT Id, Name, MemberKey, CreateDate FROM {_memberTable} WHERE MemberKey = @MemberKey";
            var result = await _readDbConnection.QuerySingleOrDefaultAsync<Member>(query, new { MemberKey = model.Value });

            if (result == null)
                return new ErrorDataResult<Member>(result);

            var resourceQuery = "SELECT r.*, m.Id AS MemId, m.* " +
                                $"FROM {_resourceTable} r " +
                                $"INNER JOIN {_memberTable} m ON m.Id = r.MemberId " +
                                "WHERE r.Status = @Status AND m.MemberKey=@MemberKey";

            var resourceResult = await _dbContext.Connection.QueryAsync<Resource>(sql: resourceQuery,
                                                                                  param: new { Status = 1, MemberKey = result.MemberKey });

            var filteredResult = resourceResult.DistinctBy(r => r.Id).ToList();
            result.Resources = filteredResult;
            return new DataResult<Member>(result);
        }
    }
}
