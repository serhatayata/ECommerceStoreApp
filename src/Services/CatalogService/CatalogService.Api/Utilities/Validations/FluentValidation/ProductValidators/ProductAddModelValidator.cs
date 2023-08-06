using CatalogService.Api.Models.ProductModels;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.ProductValidators
{
    public class ProductAddModelValidator : AbstractValidator<ProductAddModel>
    {
        public ProductAddModelValidator()
        {
            
        }
    }
}
