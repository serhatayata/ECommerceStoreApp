using FluentValidation;
using IdentityServer.Api.Models.Base.Concrete;
using IdentityServer.Api.Models.UserModels;

namespace IdentityServer.Api.Validations.FluentValidation.DefaultValidations
{
    public class StringModelValidator : AbstractValidator<StringModel>
    {
        public StringModelValidator()
        {
            RuleFor(x => x.Value).NotEmpty().WithMessage("Value cannot be empty");
            RuleFor(x => x.Value).NotNull().WithMessage("Value cannot be null");
        }
    }
}
