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
            string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

            #region Username
            RuleFor(x => x.Username).NotEmpty().NotNull().WithMessage(l => localizer[culture, "userloginmodel.username.notempty"]);
            RuleFor(x => x.Username).Length(6, 50).WithMessage(l => localizer[culture, "userloginmodel.username.length"]);
            #endregion
            #region Password
            RuleFor(x => x.Password).PasswordWithoutMessage().WithMessage(l => localizer[culture, "userloginmodel.password.notvalidpassword"]);
            RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage(l => localizer[culture, "userloginmodel.password.notnull"]);
            RuleFor(x => x.Password).Length(8, 21).WithMessage(l => localizer[culture, "userloginmodel.password.length", 8, 21]);
            RuleFor(x => x.Password).Equal(s => s.ConfirmPassword).WithMessage(l => localizer[culture, "userloginmodel.password.notequal"]);
            #endregion
            #region ConfirmPassword
            RuleFor(x => x.ConfirmPassword).NotEmpty().NotNull().WithMessage(l => localizer[culture, "userloginmodel.confirmpassword.notempty"]);
            #endregion
        }
    }
}