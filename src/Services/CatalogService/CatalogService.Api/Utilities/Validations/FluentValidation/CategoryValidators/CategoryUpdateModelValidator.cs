using CatalogService.Api.Models.CategoryModels;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.CategoryValidators
{
    public class CategoryUpdateModelValidator : AbstractValidator<CategoryUpdateModel>
    {
        public CategoryUpdateModelValidator()
        {
            
        }
    }
}
