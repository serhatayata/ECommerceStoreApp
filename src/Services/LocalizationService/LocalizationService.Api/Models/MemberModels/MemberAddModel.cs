using LocalizationService.Api.Models.Base.Abstract;

namespace LocalizationService.Api.Models.MemberModels
{
    public class MemberAddModel : IAddModel
    {
        public string Name { get; set; }
    }
}
