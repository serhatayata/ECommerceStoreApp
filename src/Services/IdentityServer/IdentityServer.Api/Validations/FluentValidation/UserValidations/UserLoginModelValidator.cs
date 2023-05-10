using FluentValidation;
using IdentityServer.Api.Extensions;
using IdentityServer.Api.Models.UserModels;
using Nest;

namespace IdentityServer.Api.Validations.FluentValidation.UserValidations
{
    public class UserLoginModelValidator : AbstractValidator<UserLoginModel>
    {
        public UserLoginModelValidator()
        {
            #region Username
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username cannot be empty");
            RuleFor(x => x.Username).NotNull().WithMessage("Username cannot be null");
            RuleFor(x => x.Username).Length(6, 50).WithMessage("Username length must be between 6-50");
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