using FluentValidation;
using IdentityServer.Api.Extensions;
using IdentityServer.Api.Models.UserModels;
using IdentityServer.Api.Services.Localization.Abstract;

namespace IdentityServer.Api.Validations.FluentValidation.UserValidations
{
    public class UserUpdateModelValidator : AbstractValidator<UserUpdateModel>
    {
        public UserUpdateModelValidator(ILocalizationService localizer,
                                        IHttpContextAccessor httpContextAccessor)
        {
            string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

            #region Name
            RuleFor(x => x.Name).NotEmpty().NotNull().WithMessage(l => localizer[culture, "userupdatemodel.name.notempty"]);
            RuleFor(x => x.Name).Length(2, 50).WithMessage(l => localizer[culture, "userupdatemodel.name.length",2,50]);
            #endregion
            #region Surname
            RuleFor(x => x.Surname).NotEmpty().NotNull().WithMessage(l => localizer[culture, "userupdatemodel.surname.notempty"]);
            RuleFor(x => x.Surname).Length(2, 50).WithMessage(l => localizer[culture, "userupdatemodel.surname.length",2,50]);
            #endregion
            #region UserName
            RuleFor(x => x.UserName).NotEmpty().NotNull().WithMessage(l => localizer[culture, "userupdatemodel.username.notempty"]);
            RuleFor(x => x.UserName).Length(8, 50).WithMessage(l => localizer[culture, "userupdatemodel.username.length",8,50]);
            #endregion
            #region Email
            RuleFor(x => x.CurrentUserName).NotEmpty().NotNull().WithMessage(l => localizer[culture, "userupdatemodel.currentusername.notempty"]);
            #endregion
        }
    }
}
