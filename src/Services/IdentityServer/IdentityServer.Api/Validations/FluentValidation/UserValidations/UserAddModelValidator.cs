using FluentValidation;
using IdentityServer.Api.Extensions;
using IdentityServer.Api.Models.UserModels;

namespace IdentityServer.Api.Validations.FluentValidation.UserValidations
{
    public class UserAddModelValidator : AbstractValidator<UserAddModel>
    {
        public UserAddModelValidator()
        {
            #region Name
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(x => x.Name).NotNull().WithMessage("Name cannot be null");
            RuleFor(x => x.Name).Length(2, 50).WithMessage("Name length must be between 2-50");
            #endregion
            #region Surname
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Surname cannot be empty");
            RuleFor(x => x.Surname).NotNull().WithMessage("Surname cannot be null");
            RuleFor(x => x.Surname).Length(2, 50).WithMessage("Surname length must be between 2-50");
            #endregion
            #region UserName
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username cannot be empty");
            RuleFor(x => x.UserName).NotNull().WithMessage("Username cannot be null");
            RuleFor(x => x.UserName).Length(8, 50).WithMessage("Username length must be between 8-50");
            #endregion
            #region Email
            RuleFor(x => x.Email).EmailAddress().WithMessage("E-mail address is not correct");
            #endregion
            #region PhoneNumber
            RuleFor(x => x.PhoneNumber).InternationalPhoneNumber().WithMessage("Phone number format is not correct +XXX XXX XX XX");
            #endregion
            #region Password
            RuleFor(x => x.Password).PasswordWithoutMessage().WithMessage("Please enter your password in terms of the rules");
            RuleFor(x => x.Password).NotNull().WithMessage("Password cannot be null");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password cannot be empty");
            RuleFor(x => x.Password).Length(8, 21).WithMessage("Password length must be between 8-21");
            RuleFor(x => x.Password).Equal(s => s.ConfirmPassword).WithMessage("Password must be equal to confirm password");
            #endregion
            #region ConfirmPassword
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Confirm password cannot be empty");
            RuleFor(x => x.ConfirmPassword).NotNull().WithMessage("Confirm password cannot be null");
            #endregion
        }
    }
}
