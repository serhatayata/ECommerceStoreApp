using CampaignService.Api.Entities;
using CampaignService.Api.Infrastructure.Contexts;
using CampaignService.Api.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace CampaignService.Api.Repository.Concrete;

public class CampaignSourceRepository : ICampaignSourceRepository
{
    private readonly CampaignDbContext _context;
    private ILogger<CampaignSourceRepository> _logger;

    public CampaignSourceRepository(
        CampaignDbContext context,
        ILogger<CampaignSourceRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<CampaignSource?> CreateAsync(CampaignSource model)
    {
        _context.Connection.Open();
        using (var transaction = _context.Connection.BeginTransaction())
        {
            try
            {
                _context.Database.UseTransaction(transaction as DbTransaction);

                await _context.CampaignSources.AddAsync(model);

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

    public async Task<CampaignSource?> UpdateAsync(CampaignSource model)
    {
        _context.Connection.Open();
        using (var transaction = _context.Connection.BeginTransaction())
        {
            try
            {
                _context.Database.UseTransaction(transaction as DbTransaction);

                var result = await _context.CampaignSources.AsNoTracking()
                                           .Where(c => c.Id == model.Id)
                                           .ExecuteUpdateAsync(c => c
                                           .SetProperty(p => p.EntityId, model.EntityId)
                                           .SetProperty(b => b.CampaignId, model.CampaignId));

                await _context.SaveChangesAsync();

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

                var result = await _context.CampaignSources
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

    public async Task<CampaignSource?> GetAsync(int id) => 
        await _context.CampaignSources.FirstOrDefaultAsync(c => c.Id == id);

    public async Task<List<CampaignSource>> GetAllAsync() => 
        await _context.CampaignSources.ToListAsync();

    public async Task<List<CampaignSource>> GetAllByCampaignIdAsync(int id) =>
        await _context.CampaignSources.Where(c => c.CampaignId == id).ToListAsync();
}
