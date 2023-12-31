using CampaignService.Api.Entities;
using CampaignService.Api.Extensions;
using CampaignService.Api.Infrastructure.Contexts;
using CampaignService.Api.Repository.Abstract;
using CampaignService.Api.Services.Cache.Abstract;
using CampaignService.Api.Utilities.Json;
using GraphQLParser;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text.Json;

namespace CampaignService.Api.Repository.Concrete;

public class CampaignItemRepository : ICampaignItemRepository
{
    private readonly CampaignDbContext _context;
    private readonly IRedisService _redisService;
    private readonly ILogger<CampaignItemRepository> _logger;

    public CampaignItemRepository(
        CampaignDbContext context,
        IRedisService redisService,
        ILogger<CampaignItemRepository> logger)
    {
        _context = context;
        _redisService = redisService;
        _logger = logger;
    }

    public async Task<CampaignItem?> CreateAsync(CampaignItem model)
    {
        _context.Connection.Open();
        using (var transaction = _context.Connection.BeginTransaction())
        {
            try
            {
                model.Code = DataGenerationExtensions.RandomCode(10);
                _context.Database.UseTransaction(transaction as DbTransaction);

                await _context.CampaignItems.AddAsync(model);

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

    public async Task<CampaignItem> UpdateAsync(CampaignItem model)
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
                _logger.LogError(ex, "ERROR - {Message}", ex.Message);
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

    public async Task<List<CampaignItem>> GetAllByRulesync(string rule)
    {
        try
        {
            var jsonDocument = JsonDocument.Parse(rule);
            var parser = new JsonExpressionParser();
            var expression = parser.ParseExpressionOf<CampaignItem>(jsonDocument);

            var result = await _context.CampaignItems
                                       .Where(expression)
                                       .ToListAsync();

            return result;
        }
        catch (Exception ex)
        {
            throw new FormatException("Error", ex);
        }
    }
}
