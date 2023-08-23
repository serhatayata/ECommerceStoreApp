using CatalogService.Api.Extensions;
using CatalogService.Api.Models.CategoryModels;
using CatalogService.Api.Services.Localization.Abstract;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.CategoryValidators
{
    public class CategoryUpdateModelValidator : AbstractValidator<CategoryUpdateModel>
    {
        public CategoryUpdateModelValidator(ILocalizationService localizer,
                                            IHttpContextAccessor httpContextAccessor)
        {
            string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

            RuleFor(b => b.Id).NotEmpty().NotNull().WithMessage(localizer[culture, "categoryupdatemodel.id.notempty"]);
            RuleFor(b => b.Id).GreaterThan(0).WithMessage(localizer[culture, "categoryupdatemodel.id.greater", 0]);

            RuleFor(b => b.Name).NotEmpty().NotNull().WithMessage(localizer[culture, "categoryupdatemodel.name.notempty"]);
            RuleFor(b => b.Name).Length(2, 500).WithMessage(localizer[culture, "categoryupdatemodel.name.length", 2, 500]);

            RuleFor(b => b.Line).NotEmpty().NotNull().WithMessage(localizer[culture, "categoryupdatemodel.line.notempty"]);
            RuleFor(b => b.Line).GreaterThan(0).WithMessage(localizer[culture, "categoryupdatemodel.line.greater", 0]);
        }
    }
}
