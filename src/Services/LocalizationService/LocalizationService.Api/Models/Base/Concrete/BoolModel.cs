using LocalizationService.Api.Models.Base.Abstract;

namespace LocalizationService.Api.Models.Base.Concrete
{
    public class BoolModel : IModel
    {
        public bool Value { get; set; }
    }
}
