using CatalogService.Api.Models.Base.Abstract;

namespace CatalogService.Api.Models.Base.Concrete
{
    public class BoolModel : IModel
    {
        public BoolModel()
        {
            
        }

        public BoolModel(bool value)
        {
            this.Value = value;
        }

        public bool Value { get; set; }
    }
}
