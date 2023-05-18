using LocalizationService.Api.Models.Base.Abstract;

namespace LocalizationService.Api.Models.Base.Concrete
{
    public class IntModel : IModel, IDeleteModel
    {
        public int Id { get; set; }
    }
}
