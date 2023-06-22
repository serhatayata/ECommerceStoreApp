using FluentValidation;
using IdentityServer.Api.Extensions;
using IdentityServer.Api.Models.UserModels;
using IdentityServer.Api.Services.Localization.Abstract;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Api.Validations.FluentValidation.UserValidations
{
    public class UserAddModelValidator : AbstractValidator<UserAddModel>
    {
        public UserAddModelValidator(ILocalizationService localizer,
                                     IHttpContextAccessor httpContextAccessor)
        {
            string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

            #region Name
            RuleFor(x => x.Name).NotEmpty().NotNull().WithMessage(localizer[culture, "useraddmodel.name.notempty"]);
            RuleFor(x => x.Name).Length(2, 50).WithMessage(localizer[culture, "useraddmodel.name.length", 2, 50]);
            #endregion
            #region Surname
            RuleFor(x => x.Surname).NotEmpty().NotNull().WithMessage(localizer[culture, "useraddmodel.surname.notempty"]);
            RuleFor(x => x.Surname).Length(2, 50).WithMessage(localizer[culture, "useraddmodel.name.length", 2, 50]);
            #endregion
            #region UserName
            RuleFor(x => x.UserName).NotEmpty().NotNull().WithMessage(localizer[culture, "useraddmodel.username.notempty"]);
            RuleFor(x => x.UserName).Length(8, 50).WithMessage(localizer[culture, "useraddmodel.name.length", 8, 50]);
            #endregion
            #region Email
            RuleFor(x => x.Email).EmailAddress().WithMessage(localizer[culture, "useraddmodel.email.notvalid"]);
            #endregion
            #region PhoneNumber
            RuleFor(x => x.PhoneNumber).InternationalPhoneNumber().WithMessage(localizer[culture, "useraddmodel.phonenumber.invalidphonenumber"]);
            #endregion
            #region Password
            RuleFor(x => x.Password).PasswordWithoutMessage().WithMessage(localizer[culture, "useraddmodel.password.notvalid"]);
            RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage(localizer[culture, "useraddmodel.password.notnull"]);
            RuleFor(x => x.Password).Length(8, 21).WithMessage(localizer[culture, "useraddmodel.password.length",8,21]);
            RuleFor(x => x.Password).Equal(s => s.ConfirmPassword).WithMessage(localizer[culture, "useraddmodel.password.notequal"]);
            #endregion
            #region ConfirmPassword
            RuleFor(x => x.ConfirmPassword).NotEmpty().NotNull().WithMessage(localizer[culture, "useraddmodel.confirmpassword.notempty"]);
            #endregion
        }
    }
}
