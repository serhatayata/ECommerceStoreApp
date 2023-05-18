using LocalizationService.Api.Models.Base.Abstract;

namespace LocalizationService.Api.Models.Base.Concrete
{
    public class PropertyModel : IModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
