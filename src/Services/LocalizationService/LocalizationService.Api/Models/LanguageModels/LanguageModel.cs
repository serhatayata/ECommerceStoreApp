using LocalizationService.Api.Models.Base.Abstract;

namespace LocalizationService.Api.Models.LanguageModels
{
    public class LanguageModel : IModel
    {
        public string Code { get; set; }
        public string DisplayName { get; set; }
    }
}
