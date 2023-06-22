using FluentValidation;
using IdentityServer.Api.Extensions;
using IdentityServer.Api.Models.ApiScopeModels;
using IdentityServer.Api.Services.Localization.Abstract;

namespace IdentityServer.Api.Validations.FluentValidation.ClientValidations
{
    public class ApiScopeAddModelValidator : AbstractValidator<ApiScopeAddModel>
    {
        public ApiScopeAddModelValidator(ILocalizationService localizer,
                                         IHttpContextAccessor httpContextAccessor)
        {
            string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

            #region Name
            RuleFor(x => x.Name).NotEmpty().NotNull().WithMessage(localizer[culture, "apiscopeaddmodel.name.notempty"]);
            RuleFor(x => x.Name).Length(5, 500).WithMessage(localizer[culture, "apiscopeaddmodel.name.length", 5, 500]);
            #endregion
            #region DisplayName
            RuleFor(x => x.DisplayName).NotEmpty().WithMessage(localizer[culture, "apiscopeaddmodel.displayname.notempty"]);
            RuleFor(x => x.DisplayName).NotNull().WithMessage(localizer[culture, "apiscopeaddmodel.displayname.notnull"]);
            RuleFor(x => x.DisplayName).Length(5, 500).WithMessage(localizer[culture, "apiscopeaddmodel.displayname.length", 5, 500]);
            #endregion
        }
    }
}
