using CampaignService.Api.Entities;
using CampaignService.Api.Infrastructure.Contexts;
using CampaignService.Api.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Linq.Expressions;

namespace CampaignService.Api.Repository.Concrete;

public class CampaignItemRepository : ICampaignItemRepository
{
    private readonly CampaignDbContext _context;

    public CampaignItemRepository(CampaignDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Create(CampaignItem model)
    {
        _context.Connection.Open();
        using (var transaction = _context.Connection.BeginTransaction())
        {
            try
            {
                _context.Database.UseTransaction(transaction as DbTransaction);

                await _context.CampaignItems.AddAsync(model);

                var result = _context.SaveChanges();

                if (result < 1)
                    return false;

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                _context.Connection.Close();
            }
        }
    }

    public async Task<CampaignItem> Update(CampaignItem model)
    {
        _context.Connection.Open();
        using (var transaction = _context.Connection.BeginTransaction())
        {
            try
            {
                _context.Database.UseTransaction(transaction as DbTransaction);

                var result = await _context.CampaignItems.Where(c => c.Id == model.Id)
                                           .ExecuteUpdateAsync(c => c
                                           .SetProperty(p => p.UserId, model.UserId)
                                           .SetProperty(b => b.Description, model.Description)
                                           .SetProperty(b => b.ExpirationDate, model.ExpirationDate)
                                           .SetProperty(b => b.Status, model.Status));

                _context.SaveChanges();

                transaction.Commit();
                return await _context.CampaignItems.FirstOrDefaultAsync(c => c.Id == model.Id);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                _context.Connection.Close();
            }
        }
    }

    public async Task<bool> Delete(int id)
    {
        _context.Connection.Open();
        using (var transaction = _context.Connection.BeginTransaction())
        {
            try
            {
                _context.Database.UseTransaction(transaction as DbTransaction);

                var result = await _context.CampaignItems
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
                throw new Exception(ex.Message);
            }
            finally
            {
                _context.Connection.Close();
            }
        }
    }

    public async Task<CampaignItem?> GetAsync(int id) => 
        await _context.CampaignItems.FirstOrDefaultAsync(c => c.Id == id);

    public async Task<CampaignItem?> GetAsync(Expression<Func<CampaignItem, bool>> predicate) => 
        await _context.CampaignItems.FirstOrDefaultAsync(predicate);

    public async Task<List<CampaignItem>> GetAllAsync() => 
        await _context.CampaignItems.ToListAsync();

    public async Task<List<CampaignItem>> GetAllByCampaignIdAsync(int id) => 
        await _context.CampaignItems.Where(c => c.CampaignId == id).ToListAsync();
}
