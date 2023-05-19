using LocalizationService.Api.Models.Base.Abstract;

namespace LocalizationService.Api.Models.LanguageModels
{
    public class LanguageUpdateModel : IUpdateModel
    {
        public string Code { get; set; }
        public string DisplayName { get; set; }
    }
}
