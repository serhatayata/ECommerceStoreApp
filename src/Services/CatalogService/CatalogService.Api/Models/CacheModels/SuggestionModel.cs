namespace CatalogService.Api.Models.CacheModels;

public class SuggestionModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string SuggestedName { get; set; }
    public double Score { get; set; }
}
