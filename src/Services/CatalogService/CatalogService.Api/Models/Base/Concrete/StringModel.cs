using CatalogService.Api.Models.Base.Abstract;

namespace CatalogService.Api.Models.Base.Concrete;

public class StringModel : IModel, IDeleteModel
{
    public StringModel()
    {
        
    }

    public StringModel(string value)
    {
        this.Value = value;
    }

    public string Value { get; set; }
}
