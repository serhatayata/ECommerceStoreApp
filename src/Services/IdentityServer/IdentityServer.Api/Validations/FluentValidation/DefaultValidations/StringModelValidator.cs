using FluentValidation;
using IdentityServer.Api.Extensions;
using IdentityServer.Api.Models.Base.Concrete;
using IdentityServer.Api.Models.UserModels;
using IdentityServer.Api.Services.Localization.Abstract;

namespace IdentityServer.Api.Validations.FluentValidation.DefaultValidations
{
    public class StringModelValidator : AbstractValidator<StringModel>
    {
        public StringModelValidator(ILocalizationService localizer,
                                    IHttpContextAccessor httpContextAccessor)
        {
            string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

            RuleFor(x => x.Value).NotEmpty().NotNull().WithMessage(l => localizer[culture, "stringmodel.value.notempty"]);
        }
    }
}
