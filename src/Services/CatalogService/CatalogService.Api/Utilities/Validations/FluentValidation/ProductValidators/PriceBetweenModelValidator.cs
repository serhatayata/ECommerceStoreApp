using CatalogService.Api.Models.ProductModels;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.ProductValidators
{
    public class PriceBetweenModelValidator : AbstractValidator<PriceBetweenModel>
    {
        public PriceBetweenModelValidator()
        {
            
        }
    }
}
