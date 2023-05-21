using LocalizationService.Api.Models.Base.Abstract;

namespace LocalizationService.Api.Models.MemberModels
{
    public class MemberUpdateModel : IUpdateModel
    {
        public string MemberKey { get; set; }
        public string Name { get; set; }
    }
}
