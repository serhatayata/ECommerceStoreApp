using LocalizationService.Api.Models.Base.Abstract;

namespace LocalizationService.Api.Models.Base.Concrete
{
    public class StringModel : IModel, IDeleteModel
    {
        public string Value { get; set; }
    }
}
