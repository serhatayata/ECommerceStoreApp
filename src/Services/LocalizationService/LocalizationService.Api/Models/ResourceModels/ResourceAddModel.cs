using LocalizationService.Api.Models.Base.Abstract;

namespace LocalizationService.Api.Models.ResourceModels
{
    public class ResourceAddModel : IAddModel
    {
        public int LanguageId { get; set; }
        public string LanguageCode { get; set; }
        public int MemberId { get; set; }
        public string Value { get; set; }
        public string Tag { get; set; }
    }
}
