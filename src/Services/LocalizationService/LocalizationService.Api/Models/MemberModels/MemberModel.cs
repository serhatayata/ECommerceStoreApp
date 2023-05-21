using LocalizationService.Api.Entities;
using LocalizationService.Api.Models.Base.Abstract;

namespace LocalizationService.Api.Models.MemberModels
{
    public class MemberModel : IModel
    {
        public string Name { get; set; }
        public string MemberKey { get; set; }
        public DateTime CreateDate { get; set; }

        public IReadOnlyList<Resource> Resources { get; set; }
    }
}
