using CatalogService.Api.Models.CategoryModels;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.CategoryValidators
{
    public class CategoryAddModelValidator : AbstractValidator<CategoryAddModel>
    {
        public CategoryAddModelValidator()
        {
            
        }
    }
}
