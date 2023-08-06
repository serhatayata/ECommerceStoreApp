using CatalogService.Api.Models.BrandModels;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.BrandValidators
{
    public class BrandUpdateModelValidator : AbstractValidator<BrandUpdateModel>
    {
        public BrandUpdateModelValidator()
        {
            
        }
    }
}
