using FluentValidation;
using IdentityServer.Api.Extensions;
using IdentityServer.Api.Models.IdentityResourceModels;
using IdentityServer.Api.Services.Localization.Abstract;

namespace IdentityServer.Api.Validations.FluentValidation.ClientValidations
{
    public class IdentityResourceAddModelValidator : AbstractValidator<IdentityResourceAddModel>
    {
        public IdentityResourceAddModelValidator(ILocalizationService localizer,
                                                 IHttpContextAccessor httpContextAccessor)
        {
            string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

            #region Name
            RuleFor(x => x.Name).NotEmpty().NotNull().WithMessage(localizer[culture, "identityresourceaddmodel.name.notempty"]);
            RuleFor(x => x.Name).Length(5, 500).WithMessage(localizer[culture, "identityresourceaddmodel.name.length", 5, 500]);
            #endregion
            #region DisplayName
            RuleFor(x => x.DisplayName).NotEmpty().NotNull().WithMessage(localizer[culture, "identityresourceaddmodel.displayname.notempty"]);
            RuleFor(x => x.DisplayName).Length(5, 500).WithMessage(localizer[culture, "identityresourceaddmodel.displayname.length", 5, 500]);
            #endregion
        }
    }
}
