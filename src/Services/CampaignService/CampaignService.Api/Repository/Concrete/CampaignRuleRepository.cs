using CampaignService.Api.Entities;
using CampaignService.Api.Infrastructure.Contexts;
using CampaignService.Api.Models.CampaignRule;
using CampaignService.Api.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace CampaignService.Api.Repository.Concrete;

public class CampaignRuleRepository : ICampaignRuleRepository
{
    private readonly CampaignDbContext _context;
    private readonly ILogger<CampaignRuleRepository> _logger;

    public CampaignRuleRepository(
        CampaignDbContext context, 
        ILogger<CampaignRuleRepository> logger)
    {
        _context = context;
        _logger = logger;
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
                _logger.LogError(ex, "ERROR - {Message}", ex.Message);
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
                                           .SetProperty(p => p.Type, model.Type)
                                           .SetProperty(p => p.Data, model.Data)
                                           .SetProperty(p => p.Value, model.Value));

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
                _logger.LogError(ex, "ERROR - {Message}", ex.Message);
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

    public async Task<List<CampaignRule>> GetAllByFilterAsync(Expression<Func<CampaignRule, bool>> expression) =>
        await _context.CampaignRules.Where(expression).ToListAsync();
}
