using CatalogService.Api.Models.Base.Abstract;

namespace CatalogService.Api.Models.Base.Concrete
{
    public class BoolModel : IModel
    {
        public bool Value { get; set; }
    }
}
