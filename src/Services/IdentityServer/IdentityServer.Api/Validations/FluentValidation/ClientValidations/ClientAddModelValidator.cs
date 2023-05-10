using FluentValidation;
using IdentityServer.Api.Models.ClientModels;
using IdentityServer.Api.Models.UserModels;

namespace IdentityServer.Api.Validations.FluentValidation.ClientValidations
{
    public class ClientAddModelValidator : AbstractValidator<ClientAddModel>
    {
        public ClientAddModelValidator()
        {
            
        }
    }
}
