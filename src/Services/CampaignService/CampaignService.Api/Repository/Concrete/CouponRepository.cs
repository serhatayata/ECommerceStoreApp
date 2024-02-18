using CampaignService.Api.Entities;
using CampaignService.Api.Infrastructure.Contexts;
using CampaignService.Api.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Nest;
using System.Data.Common;
using System.Linq.Expressions;

namespace CampaignService.Api.Repository.Concrete;

public class CouponRepository : ICouponRepository
{
    private readonly CampaignDbContext _context;
    private readonly ILogger<CouponRepository> _logger;

    public CouponRepository(
        CampaignDbContext context,
        ILogger<CouponRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Coupon?> CreateAsync(Coupon model)
    {
        _context.Connection.Open();
        using (var transaction = _context.Connection.BeginTransaction())
        {
            try
            {
                _context.Database.UseTransaction(transaction as DbTransaction);

                await _context.Coupons.AddAsync(model);

                var result = _context.SaveChanges();

                if (result < 1)
                    return null;

                transaction.Commit();
                return model;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "ERROR - {Message}", ex.Message);
                throw new Exception(ex.Message);
            }
            finally
            {
                _context.Connection.Close();
            }
        }
    }

    public async Task<Coupon?> UpdateAsync(Coupon model)
    {
        _context.Connection.Open();
        using (var transaction = _context.Connection.BeginTransaction())
        {
            try
            {
                _context.Database.UseTransaction(transaction as DbTransaction);

                var result = await _context.Coupons.Where(c => c.Id == model.Id)
                                           .ExecuteUpdateAsync(c => c
                                           .SetProperty(b => b.Name, model.Name)
                                           .SetProperty(b => b.Description, model.Description)
                                           .SetProperty(b => b.Type, model.Type)
                                           .SetProperty(b => b.UsageType, model.UsageType)
                                           .SetProperty(b => b.CalculationType, model.CalculationType)
                                           .SetProperty(b => b.CalculationAmount, model.CalculationAmount)
                                           .SetProperty(b => b.Amount, model.Amount)
                                           .SetProperty(b => b.MaxUsage, model.MaxUsage)
                                           .SetProperty(b => b.UsageCount, model.UsageCount)
                                           .SetProperty(b => b.ExpirationDate, model.ExpirationDate));

                _context.SaveChanges();

                transaction.Commit();
                return model;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "ERROR - {Message}", ex.Message);
                throw new Exception(ex.Message);
            }
            finally
            {
                _context.Connection.Close();
            }
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        _context.Connection.Open();
        using (var transaction = _context.Connection.BeginTransaction())
        {
            try
            {
                _context.Database.UseTransaction(transaction as DbTransaction);

                var result = await _context.Coupons
                                           .Where(c => c.Id == id)
                                           .ExecuteDeleteAsync();

                _context.SaveChanges();

                if (result < 1)
                    return false;

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "ERROR - {Message}", ex.Message);
                throw new Exception(ex.Message);
            }
            finally
            {
                _context.Connection.Close();
            }
        }
    }

    public async Task<Coupon?> GetAsync(int id) =>
        await _context.Coupons.FirstOrDefaultAsync(c => c.Id == id);

    public async Task<List<Coupon>> GetAllAsync() =>
        await _context.Coupons.ToListAsync();

    public async Task<List<Coupon>> GetAllByFilterAsync(Expression<Func<Coupon, bool>> expression) =>
        await _context.Coupons.Where(expression).ToListAsync();
}