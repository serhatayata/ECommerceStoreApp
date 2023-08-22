using EventBus.Base.Attributes;
using EventBus.Base.Events;

namespace CatalogService.Api.IntegrationEvents.Events;

[DeadLetter]
public class ProductUpdatedIntegrationEvent : IntegrationEvent
{
    public ProductUpdatedIntegrationEvent()
    {
        
    }
    public ProductUpdatedIntegrationEvent(
        int id, 
        string name, 
        string nameSuggest, 
        string description, 
        string link, 
        decimal price, 
        int availableStock, 
        DateTime createDate, 
        DateTime updateDate, 
        int? productTypeId, 
        int? brandId)
    {
        Id = id;
        Name = name;
        NameSuggest = nameSuggest;
        Description = description;
        Link = link;
        Price = price;
        AvailableStock = availableStock;
        CreateDate = createDate;
        UpdateDate = updateDate;
        ProductTypeId = productTypeId;
        BrandId = brandId;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string NameSuggest { get; set; }
    public string Description { get; set; }
    public string Link { get; set; }
    public decimal Price { get; set; }
    public int AvailableStock { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public int? ProductTypeId { get; set; }
    public int? BrandId { get; set; }
}
