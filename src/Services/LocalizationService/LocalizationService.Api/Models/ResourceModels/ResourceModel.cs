using LocalizationService.Api.Models.Base.Abstract;
using LocalizationService.Api.Models.LanguageModels;
using LocalizationService.Api.Models.MemberModels;

namespace LocalizationService.Api.Models.ResourceModels
{
    public class ResourceModel : IModel
    {
        public string LanguageCode { get; set; }
        public string MemberKey { get; set; }
        public string Tag { get; set; }
        public string Value { get; set; }
        public string ResourceCode { get; set; }

        public LanguageModel Language { get; set; }
        public MemberModel Member { get; set; }
    }
}
