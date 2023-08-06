using CatalogService.Api.Models.ProductModels;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.ProductValidators
{
    public class ProductUpdateModelValidator : AbstractValidator<ProductUpdateModel>
    {
        public ProductUpdateModelValidator()
        {
            
        }
    }
}
