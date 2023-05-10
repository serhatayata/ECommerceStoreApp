using FluentValidation;
using IdentityServer.Api.Models.UserModels;

namespace IdentityServer.Api.Validations.FluentValidation.UserValidations
{
    public class UserUpdateModelValidator : AbstractValidator<UserUpdateModel>
    {
        public UserUpdateModelValidator()
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
            RuleFor(x => x.CurrentUserName).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(x => x.CurrentUserName).NotNull().WithMessage("Name cannot be null");
            #endregion
        }
    }
}
