using CatalogService.Api.Models.Base.Abstract;

namespace CatalogService.Api.Models.Base.Concrete;

public class StringModel : IModel, IDeleteModel
{
    public string Value { get; set; }
}
