using LocalizationService.Api.Data.Contexts;
using LocalizationService.Api.Data.Repositories.EntityFramework.Abstract;
using LocalizationService.Api.Entities;
using LocalizationService.Api.Models.Base.Concrete;
using LocalizationService.Api.Utilities.Results;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace LocalizationService.Api.Data.Repositories.EntityFramework.Concrete
{
    public class EfMemberRepository : IEfMemberRepository
    {
        private readonly ILocalizationDbContext _dbContext;

        public EfMemberRepository(ILocalizationDbContext dbContext)
        {
            _dbContext = dbContext;
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

                var addedMember = await _dbContext.Members.AddAsync(entity);
                var addResult = await _dbContext.SaveChangesAsync(default);

                if (addResult < 1)
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
                _dbContext.Database.UseTransaction(transaction as DbTransaction);

                var memberExists = await _dbContext.Members.FirstOrDefaultAsync(l => l.MemberKey == model.Value);
                if (memberExists == null)
                    return new ErrorResult("Member does not exist");

                var deletedLangugage = _dbContext.Members.Remove(memberExists);
                var deleteResult = await _dbContext.SaveChangesAsync(default);

                if (deleteResult < 1)
                    return new ErrorResult("Member not added");

                transaction.Commit();

                return deleteResult > 0 ? new SuccessResult() : new ErrorResult("Member not added");
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
                _dbContext.Database.UseTransaction(transaction as DbTransaction);

                var memberExists = await _dbContext.Members.FirstOrDefaultAsync(l => l.MemberKey == entity.MemberKey);
                if (memberExists == null)
                    return new ErrorResult("Member does not exist");

                if (memberExists.Name == entity.Name)
                    return new SuccessResult();

                bool memberNameExists = await _dbContext.Members.AnyAsync(l => l.Id != memberExists.Id && l.Name == entity.Name);
                if (memberNameExists)
                    return new ErrorResult("Member name already exists");

                memberExists.Name = entity.Name;

                var updatedLangugage = _dbContext.Members.Update(memberExists);
                var updateResult = await _dbContext.SaveChangesAsync(default);

                if (updateResult < 1)
                    return new ErrorResult("Member not added");

                transaction.Commit();

                return updateResult > 0 ? new SuccessResult() : new ErrorResult("Member not added");
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
            var result = await _dbContext.Members.ToListAsync();
            return new DataResult<IReadOnlyList<Member>>(result);
        }

        public async Task<DataResult<Member>> GetAsync(StringModel model)
        {
            var result = await _dbContext.Members.FirstOrDefaultAsync(m => m.MemberKey == model.Value);
            return new DataResult<Member>(result);
        }
    }
}
