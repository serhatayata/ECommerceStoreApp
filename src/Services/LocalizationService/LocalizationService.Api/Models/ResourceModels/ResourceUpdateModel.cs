using LocalizationService.Api.Models.Base.Abstract;

namespace LocalizationService.Api.Models.ResourceModels
{
    public class ResourceUpdateModel : IUpdateModel
    {
        public string Tag { get; set; }
        public string Value { get; set; }
        public string ResourceCode { get; set; }
    }
}
