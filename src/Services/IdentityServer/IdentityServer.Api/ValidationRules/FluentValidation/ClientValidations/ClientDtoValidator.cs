using FluentValidation;
using IdentityServer.Api.Models.ClientModels;

namespace IdentityServer.Api.ValidationRules.FluentValidation.ClientValidations
{
    public class ClientDtoValidator : AbstractValidator<ClientAddModel>
    {
        public ClientDtoValidator()
        {
            
        }
    }
}
