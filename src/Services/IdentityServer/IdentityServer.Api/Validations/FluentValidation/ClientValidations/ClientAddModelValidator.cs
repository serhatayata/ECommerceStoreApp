using FluentValidation;
using IdentityServer.Api.Extensions;
using IdentityServer.Api.Models.ClientModels;
using IdentityServer.Api.Models.UserModels;
using IdentityServer.Api.Services.Localization.Abstract;

namespace IdentityServer.Api.Validations.FluentValidation.ClientValidations
{
    public class ClientAddModelValidator : AbstractValidator<ClientAddModel>
    {
        public ClientAddModelValidator(ILocalizationService localizer,
                                       IHttpContextAccessor httpContextAccessor)
        {
            string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

            #region ClientId
            RuleFor(x => x.ClientId).NotEmpty().NotNull().WithMessage(l => localizer[culture, "clientaddmodel.clientid.notempty"]);
            RuleFor(x => x.ClientId).Length(5, 500).WithMessage(l => localizer[culture, "clientaddmodel.clientid.length", 5, 500]);
            #endregion
            #region ClientName
            RuleFor(x => x.ClientName).NotEmpty().NotNull().WithMessage(l => localizer[culture, "clientaddmodel.clientname.notempty"]);
            RuleFor(x => x.ClientName).Length(5, 500).WithMessage(l => localizer[culture, "clientaddmodel.clientname.length", 5, 500]);
            #endregion
            #region Secrets
            RuleFor(x => x.Secrets).Must(s => s == null || s.Any()).WithMessage(l => localizer[culture, "clientaddmodel.secrets.empty"]);
            #endregion
            #region AllowedGrantTypes
            RuleFor(x => x.AllowedGrantTypes).Must(s => s == null || s.Any()).WithMessage(l => localizer[culture, "clientaddmodel.allowedgranttypes.empty"]);
            #endregion
            #region AllowedScopes
            RuleFor(x => x.AllowedScopes).Must(s => s == null || s.Any()).WithMessage(l => localizer[culture, "clientaddmodel.allowedscopes.empty"]);
            #endregion
        }
    }
}
