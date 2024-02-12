using CampaignService.Api.Entities;
using CampaignService.Api.Models.CouponItem;
using CampaignService.Api.Repository.Abstract;

namespace CampaignService.Api.Repository.Concrete;

public class CouponItemRepository : ICouponItemRepository
{
    public Task<CouponItem?> CreateAsync(CouponItem model)
    {
        throw new NotImplementedException();
    }

    public Task<CouponItem?> CreateBulkAsync(List<CouponItem> model)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<CouponItem>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<CouponItem>> GetAllByFilterAsync(CouponItemGetByFilterModel model)
    {
        throw new NotImplementedException();
    }

    public Task<CouponItem?> GetAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<CouponItem?> UpdateAsync(CouponItem model)
    {
        throw new NotImplementedException();
    }
}
