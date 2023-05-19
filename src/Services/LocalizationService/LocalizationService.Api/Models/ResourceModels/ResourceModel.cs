using LocalizationService.Api.Models.Base.Abstract;

namespace LocalizationService.Api.Models.ResourceModels
{
    public class ResourceModel : IModel
    {
        public string LanguageCode { get; set; }
        public string MemberKey { get; set; }
        public string Tag { get; set; }
        public string Value { get; set; }
        public string ResourceCode { get; set; }
    }
}
