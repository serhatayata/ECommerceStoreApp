using FluentValidation;
using IdentityServer.Api.Extensions;
using IdentityServer.Api.Models.ApiResourceModels;
using IdentityServer.Api.Services.Localization.Abstract;

namespace IdentityServer.Api.Validations.FluentValidation.ClientValidations
{
    public class ApiResourceAddModelValidator : AbstractValidator<ApiResourceAddModel>
    {
        public ApiResourceAddModelValidator(ILocalizationService localizer,
                                            IHttpContextAccessor httpContextAccessor)
        {
            string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

            RuleFor(x => x.Name).NotEmpty().NotNull().WithMessage(localizer[culture, "apiresourceaddmodel.name.notempty"]);
            RuleFor(x => x.Name).Length(5, 500).WithMessage(localizer[culture, "apiresourceaddmodel.name.length", 5, 500]);

            RuleFor(x => x.Scopes).Must(s => s == null || s.Any()).WithMessage(localizer[culture, "apiresourceaddmodel.scopes.notempty"]);

            RuleFor(x => x.Secrets).Must(s => s == null || s.Any()).WithMessage(localizer[culture, "apiresourceaddmodel.secrets.notempty"]);
        }
    }
}