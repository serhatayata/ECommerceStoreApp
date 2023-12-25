using CampaignService.Api.Entities;
using CampaignService.Api.Infrastructure.Contexts;
using CampaignService.Api.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace CampaignService.Api.Repository.Concrete;

public class CampaignRuleRepository : ICampaignRuleRepository
{
    private readonly CampaignDbContext _context;

    public CampaignRuleRepository(CampaignDbContext context)
    {
        _context = context;
    }

    public async Task<CampaignRule?> CreateAsync(CampaignRule model)
    {
        _context.Connection.Open();
        using (var transaction = _context.Connection.BeginTransaction())
        {
            try
            {
                _context.Database.UseTransaction(transaction as DbTransaction);

                await _context.CampaignRules.AddAsync(model);

                var result = _context.SaveChanges();

                if (result < 1)
                    return null;

                transaction.Commit();
                return model;
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

    public async Task<CampaignRule?> UpdateAsync(CampaignRule model)
    {
        _context.Connection.Open();
        using (var transaction = _context.Connection.BeginTransaction())
        {
            try
            {
                _context.Database.UseTransaction(transaction as DbTransaction);

                var result = await _context.CampaignRules.Where(c => c.Id == model.Id)
                                           .ExecuteUpdateAsync(c => c
                                           .SetProperty(p => p.Name, model.Name)
                                           .SetProperty(b => b.Description, model.Description)
                                           .SetProperty(b => b.Value, model.Value));

                _context.SaveChanges();

                transaction.Commit();
                return model;
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

    public async Task<bool> DeleteAsync(int id)
    {
        _context.Connection.Open();
        using (var transaction = _context.Connection.BeginTransaction())
        {
            try
            {
                _context.Database.UseTransaction(transaction as DbTransaction);

                var result = await _context.CampaignRules
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

    public async Task<CampaignRule?> GetAsync(int id) => 
        await _context.CampaignRules.FirstOrDefaultAsync(c => c.Id == id);

    public async Task<List<CampaignRule>> GetAllAsync() => 
        await _context.CampaignRules.ToListAsync();
}
