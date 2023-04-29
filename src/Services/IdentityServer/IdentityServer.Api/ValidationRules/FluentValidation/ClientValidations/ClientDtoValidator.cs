using FluentValidation;
using IdentityServer.Api.Dtos;
using IdentityServer.Api.Dtos.ClientDtos;

namespace IdentityServer.Api.ValidationRules.FluentValidation.ClientValidations
{
    public class ClientDtoValidator : AbstractValidator<ClientAddDto>
    {
        public ClientDtoValidator()
        {
            
        }
    }
}
