﻿using CampaignService.Api.Entities;
using CampaignService.Api.Infrastructure.Contexts;
using CampaignService.Api.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace CampaignService.Api.Repository.Concrete;

public class CampaignRepository : ICampaignRepository
{
    private readonly CampaignDbContext _context;
    private readonly ILogger<CampaignRepository> _logger;

    public CampaignRepository(
        CampaignDbContext context, 
        ILogger<CampaignRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Campaign?> CreateAsync(Campaign model)
    {
        _context.Connection.Open();
        using (var transaction = _context.Connection.BeginTransaction())
        {
            try
            {
                _context.Database.UseTransaction(transaction as DbTransaction);

                await _context.Campaigns.AddAsync(model);

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

    public async Task<Campaign?> UpdateAsync(Campaign model)
    {
        _context.Connection.Open();
        using (var transaction = _context.Connection.BeginTransaction())
        {
            try
            {
                _context.Database.UseTransaction(transaction as DbTransaction);

                var result = await _context.Campaigns.Where(c => c.Id == model.Id)
                                           .ExecuteUpdateAsync(c => c
                                           .SetProperty(p => p.Status, model.Status)
                                           .SetProperty(b => b.Name, model.Name)
                                           .SetProperty(b => b.Description, model.Description)
                                           .SetProperty(b => b.ExpirationDate, model.ExpirationDate)
                                           .SetProperty(b => b.StartDate, model.StartDate)
                                           .SetProperty(b => b.UpdateDate, model.UpdateDate)
                                           .SetProperty(b => b.Sponsor, model.Sponsor)
                                           .SetProperty(b => b.CampaignType, model.CampaignType)
                                           .SetProperty(b => b.CalculationType, model.CalculationType)
                                           .SetProperty(b => b.CalculationAmount, model.CalculationAmount)
                                           .SetProperty(b => b.MaxUsagePerUser, model.MaxUsagePerUser)
                                           .SetProperty(b => b.MaxUsage, model.MaxUsage)
                                           .SetProperty(b => b.IsForAllCategory, model.IsForAllCategory)
                                           .SetProperty(b => b.Amount, model.Amount));

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

                var result = await _context.Campaigns
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

    public async Task<Campaign?> GetAsync(int id) => 
        await _context.Campaigns.FirstOrDefaultAsync(c => c.Id == id);

    public async Task<List<Campaign>> GetAllAsync() => 
        await _context.Campaigns.ToListAsync();
}