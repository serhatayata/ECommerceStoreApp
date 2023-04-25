using FluentValidation;
using IdentityServer.Api.Dtos;

namespace IdentityServer.Api.ValidationRules.FluentValidation.ClientValidations
{
    public class ClientDtoValidator : AbstractValidator<ClientDto>
    {
        public ClientDtoValidator()
        {
            
        }
    }
}
