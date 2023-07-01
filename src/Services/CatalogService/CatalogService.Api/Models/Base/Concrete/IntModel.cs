using CatalogService.Api.Models.Base.Abstract;

namespace CatalogService.Api.Models.Base.Concrete;

public class IntModel : IModel, IDeleteModel
{
    public int Value { get; set; }
}
