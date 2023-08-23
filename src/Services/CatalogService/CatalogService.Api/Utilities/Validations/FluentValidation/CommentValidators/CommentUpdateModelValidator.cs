using CatalogService.Api.Extensions;
using CatalogService.Api.Models.CommentModels;
using CatalogService.Api.Services.Localization.Abstract;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.CommentValidators
{
    public class CommentUpdateModelValidator : AbstractValidator<CommentUpdateModel>
    {
        public CommentUpdateModelValidator(ILocalizationService localizer,
                                           IHttpContextAccessor httpContextAccessor)
        {
            string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

            RuleFor(b => b.Code).NotEmpty().NotNull().WithMessage(localizer[culture, "commentupdatemodel.code.notempty"]);

            RuleFor(b => b.Content).NotEmpty().NotNull().WithMessage(localizer[culture, "commentupdatemodel.content.notempty"]);
            RuleFor(b => b.Content).Length(2, 1000).WithMessage(localizer[culture, "commentupdatemodel.content.notempty", 2, 1000]);

            RuleFor(b => b.UserId).NotEmpty().NotNull().WithMessage(localizer[culture, "commentupdatemodel.userid.notempty"]);
        }
    }
}
