using EventBus.Base.Events;

namespace CatalogService.Api.IntegrationEvents.Events
{
    public class ProductUpdatedIntegrationEvent : IntegrationEvent
    {
        public ProductUpdatedIntegrationEvent(
            int id, 
            string name, 
            string description, 
            decimal price, 
            int availableStock, 
            int? productTypeId, 
            int? brandId)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            AvailableStock = availableStock;
            ProductTypeId = productTypeId;
            BrandId = brandId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int AvailableStock { get; set; }
        public int? ProductTypeId { get; set; }
        public int? BrandId { get; set; }
    }
}
