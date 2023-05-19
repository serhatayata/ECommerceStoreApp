namespace LocalizationService.Api.Models.IncludeOptions
{
    public class ResourceIncludeOptions : IBaseIncludeOptions
    {
        public bool Member { get; set; } = true;
        public bool Language { get; set; } = true;
    }
}
