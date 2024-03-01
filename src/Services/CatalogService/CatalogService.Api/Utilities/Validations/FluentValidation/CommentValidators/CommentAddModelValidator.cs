using CatalogService.Api.Extensions;
using CatalogService.Api.Models.CommentModels;
using CatalogService.Api.Services.Localization.Abstract;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.CommentValidators
{
    public class CommentAddModelValidator : AbstractValidator<CommentAddModel>
    {
        public CommentAddModelValidator(ILocalizationService localizer,
                                        IHttpContextAccessor httpContextAccessor)
        {
            string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

            RuleFor(b => b.ProductId).NotEmpty().NotNull().WithMessage(l => localizer[culture, "commentaddmodel.productid.notempty"]);
            RuleFor(b => b.ProductId).GreaterThan(0).WithMessage(l => localizer[culture, "commentaddmodel.productid.greater", 0]);

            RuleFor(b => b.Content).NotEmpty().NotNull().WithMessage(l => localizer[culture, "commentaddmodel.content.notempty"]);
            RuleFor(b => b.Content).Length(2,1000).WithMessage(l => localizer[culture, "commentaddmodel.content.length", 2, 1000]);

            RuleFor(b => b.UserId).NotEmpty().NotNull().WithMessage(l => localizer[culture, "commentaddmodel.userid.notempty"]);
        }
    }
}