using FluentValidation;
using IdentityServer.Api.Models.UserModels;

namespace IdentityServer.Api.Validations.FluentValidation.UserValidations
{
    public class UserAddModelValidator : AbstractValidator<UserAddModel>
    {
        public UserAddModelValidator()
        {
            
        }
    }
}
