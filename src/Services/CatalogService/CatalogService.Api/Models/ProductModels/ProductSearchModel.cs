using CatalogService.Api.Models.CacheModels;

namespace CatalogService.Api.Models.ProductModels;

public class ProductSearchModel
{
    public List<ProductModel> Products { get; set; }
    public List<SuggestionModel> Suggests { get; set; }
    
}
