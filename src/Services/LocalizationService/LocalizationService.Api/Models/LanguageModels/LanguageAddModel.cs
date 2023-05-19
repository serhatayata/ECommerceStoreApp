using LocalizationService.Api.Models.Base.Abstract;

namespace LocalizationService.Api.Models.LanguageModels
{
    public class LanguageAddModel : IAddModel
    {
        public string Code { get; set; }
        public string DisplayName { get; set; }
    }
}
