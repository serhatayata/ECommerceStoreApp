using CampaignService.Api.Entities;
using CampaignService.Api.Infrastructure.Contexts;
using CampaignService.Api.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Linq.Expressions;

namespace CampaignService.Api.Repository.Concrete;

public class CouponItemRepository : ICouponItemRepository
{
    private readonly CampaignDbContext _context;
    private readonly ILogger<CouponItemRepository> _logger;

    public CouponItemRepository(
        CampaignDbContext context,
        ILogger<CouponItemRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<CouponItem?> CreateAsync(CouponItem model)
    {
        _context.Connection.Open();
        using (var transaction = _context.Connection.BeginTransaction())
        {
            try
            {
                _context.Database.UseTransaction(transaction as DbTransaction);

                await _context.CouponItems.AddAsync(model);

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

    public async Task<bool> CreateBulkAsync(List<CouponItem> model)
    {
        _context.Connection.Open();
        using (var transaction = _context.Connection.BeginTransaction())
        {
            try
            {
                _context.Database.UseTransaction(transaction as DbTransaction);

                foreach (var couponItem in model)
                    await _context.CouponItems.AddAsync(couponItem);

                var result = _context.SaveChanges();

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

    public async Task<CouponItem?> UpdateAsync(CouponItem model)
    {
        _context.Connection.Open();
        using (var transaction = _context.Connection.BeginTransaction())
        {
            try
            {
                _context.Database.UseTransaction(transaction as DbTransaction);

                var result = await _context.CouponItems.Where(c => c.Id == model.Id)
                                           .ExecuteUpdateAsync(c => c
                                           .SetProperty(b => b.CouponId, model.CouponId)
                                           .SetProperty(b => b.Status, model.Status)
                                           .SetProperty(b => b.OrderId, model.OrderId));

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

                var result = await _context.CouponItems
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

    public async Task<CouponItem?> GetAsync(int id) =>
        await _context.CouponItems.FirstOrDefaultAsync(c => c.Id == id);

    public async Task<List<CouponItem>> GetAllAsync() =>
        await _context.CouponItems.ToListAsync();

    public async Task<List<CouponItem>> GetAllByFilterAsync(Expression<Func<CouponItem, bool>> expression) =>
        await _context.CouponItems.Where(expression).ToListAsync();
}
