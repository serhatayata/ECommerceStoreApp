using FluentValidation;
using IdentityServer.Api.Models.ClientModels;
using IdentityServer.Api.Models.UserModels;

namespace IdentityServer.Api.Validations.FluentValidation.ClientValidations
{
    public class ClientAddModelValidator : AbstractValidator<ClientAddModel>
    {
        public ClientAddModelValidator()
        {
            #region ClientId
            RuleFor(x => x.ClientId).NotEmpty().WithMessage("Client Id cannot be empty");
            RuleFor(x => x.ClientId).NotNull().WithMessage("Client Id cannot be null");
            RuleFor(x => x.ClientId).Length(5, 500).WithMessage("Client Id length must be between 5-500");
            #endregion
            #region ClientName
            RuleFor(x => x.ClientName).NotEmpty().WithMessage("Client name cannot be empty");
            RuleFor(x => x.ClientName).NotNull().WithMessage("Client name cannot be null");
            RuleFor(x => x.ClientName).Length(5, 500).WithMessage("Client name length must be between 5-500");
            #endregion
            #region Secrets
            RuleFor(x => x.Secrets).Must(s => s == null || s.Any()).WithMessage("Secrets cannot be empty");
            #endregion
            #region AllowedGrantTypes
            RuleFor(x => x.AllowedGrantTypes).Must(s => s == null || s.Any()).WithMessage("Allowed grant types cannot be empty");
            #endregion
            #region AllowedScopes
            RuleFor(x => x.AllowedScopes).Must(s => s == null || s.Any()).WithMessage("Allowed scopes cannot be empty");
            #endregion
        }
    }
}
