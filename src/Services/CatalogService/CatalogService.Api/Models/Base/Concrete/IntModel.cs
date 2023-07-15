using CatalogService.Api.Models.Base.Abstract;

namespace CatalogService.Api.Models.Base.Concrete;

public class IntModel : IModel, IDeleteModel
{
    public IntModel()
    {
        
    }

    public IntModel(int value)
    {
        this.Value = value;
    }

    public int Value { get; set; }
}
