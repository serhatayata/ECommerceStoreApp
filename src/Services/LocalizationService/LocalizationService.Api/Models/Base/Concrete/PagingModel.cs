using LocalizationService.Api.Models.Base.Abstract;

namespace LocalizationService.Api.Models.Base.Concrete
{
    public class PagingModel : IModel
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 8;
    }
}
