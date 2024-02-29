namespace LocalizationService.Api.Models.ResourceModels
{
    public class ResourceCacheModel
    {
        public ResourceCacheModel()
        {
            
        }

        public ResourceCacheModel(
            string tag,
            string value,
            string resourceCode,
            string languageCode,
            int memberId)
        {
            this.Tag = tag;
            this.Value = value;
            this.ResourceCode = resourceCode;
            this.LanguageCode = languageCode;
            this.MemberId = memberId;
        }

        public string Tag { get; set; }
        public string Value { get; set; }
        public string ResourceCode { get; set; }
        public string LanguageCode { get; set; }
        public int MemberId { get; set; }
    }
}
