using LocalizationService.Api.Models.Base.Abstract;

namespace LocalizationService.Api.Models.ResourceModels
{
    public class ResourceAddModel : IAddModel
    {
        public string LanguageCode { get; set; }
        public string MemberKey { get; set; }
        public string Value { get; set; }
        public string Tag { get; set; }
    }
}
