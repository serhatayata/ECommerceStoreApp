using FluentValidation;
using IdentityServer.Api.Extensions;
using IdentityServer.Api.Models.UserModels;
using IdentityServer.Api.Services.Localization.Abstract;

namespace IdentityServer.Api.Validations.FluentValidation.UserValidations
{
    public class UserLoginModelValidator : AbstractValidator<UserLoginModel>
    {
        public UserLoginModelValidator(ILocalizationService localizer, 
                                       IHttpContextAccessor httpContextAccessor)
        {
            var acceptLanguage = httpContextAccessor?.HttpContext?.Request?.GetTypedHeaders()?.AcceptLanguage?.FirstOrDefault()?.Value;
            var currentCulture = acceptLanguage.HasValue ? acceptLanguage.Value : "tr-TR";
            string culture = currentCulture.Value;

            #region Username
            RuleFor(x => x.Username).NotEmpty().WithMessage(localizer[culture, "userloginmodel.username.notempty"]);
            RuleFor(x => x.Username).NotNull().WithMessage(localizer[culture, "userloginmodel.username.notnull"]);
            RuleFor(x => x.Username).Length(6, 50).WithMessage(localizer[culture, "userloginmodel.username.length"]);
            #endregion
            #region Password
            RuleFor(x => x.Password).PasswordWithoutMessage().WithMessage(localizer[culture, "userloginmodel.password.notvalidpassword"]);
            RuleFor(x => x.Password).NotNull().WithMessage(localizer[culture, "userloginmodel.password.notnull"]);
            RuleFor(x => x.Password).NotEmpty().WithMessage(localizer[culture, "userloginmodel.password.notempty"]);
            RuleFor(x => x.Password).Length(8, 21).WithMessage(localizer[culture, "userloginmodel.password.length", 8, 21]);
            RuleFor(x => x.Password).Equal(s => s.ConfirmPassword).WithMessage(localizer[culture, "userloginmodel.password.notequal"]);
            #endregion
            #region ConfirmPassword
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage(localizer[culture, "userloginmodel.confirmpassword.notempty"]);
            RuleFor(x => x.ConfirmPassword).NotNull().WithMessage(localizer[culture, "userloginmodel.confirmpassword.notnull"]);
            #endregion
        }
    }
}