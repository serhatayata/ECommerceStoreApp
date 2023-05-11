using FluentValidation;
using IdentityServer.Api.Models.ApiResourceModels;
using IdentityServer.Api.Models.ApiScopeModels;

namespace IdentityServer.Api.Validations.FluentValidation.ClientValidations
{
    public class ApiResourceAddModelValidator : AbstractValidator<ApiResourceAddModel>
    {
        public ApiResourceAddModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(x => x.Name).NotNull().WithMessage("Name cannot be null");
            RuleFor(x => x.Name).Length(5, 500).WithMessage("Name length must be between 5-500");

            RuleFor(x => x.Scopes).Must(s => s == null || s.Any()).WithMessage("Scopes cannot be empty");

            RuleFor(x => x.Secrets).Must(s => s == null || s.Any()).WithMessage("Scopes cannot be empty");
        }
    }
}