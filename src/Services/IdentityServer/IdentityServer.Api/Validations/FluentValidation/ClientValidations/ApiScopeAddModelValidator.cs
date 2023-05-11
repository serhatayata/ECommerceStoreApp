using FluentValidation;
using IdentityServer.Api.Models.ApiScopeModels;
using IdentityServer.Api.Models.ClientModels;

namespace IdentityServer.Api.Validations.FluentValidation.ClientValidations
{
    public class ApiScopeAddModelValidator : AbstractValidator<ApiScopeAddModel>
    {
        public ApiScopeAddModelValidator()
        {
            #region Name
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(x => x.Name).NotNull().WithMessage("Name cannot be null");
            RuleFor(x => x.Name).Length(5, 500).WithMessage("Name length must be between 5-500");
            #endregion
            #region DisplayName
            RuleFor(x => x.DisplayName).NotEmpty().WithMessage("Display name cannot be empty");
            RuleFor(x => x.DisplayName).NotNull().WithMessage("Display name cannot be null");
            RuleFor(x => x.DisplayName).Length(5, 500).WithMessage("Display name length must be between 5-500");
            #endregion
        }
    }
}
