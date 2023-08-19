namespace CatalogService.Api.Models.ProductModels
{
    public class ProductSearchModel
    {
        public List<ProductModel> Products { get; set; }
        public List<ProductSuggest> Suggests { get; set; }
        
    }
}
