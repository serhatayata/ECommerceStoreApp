using IdentityServer4.Validation;

namespace IdentityServer.Api.Validations.ValidationContexts
{
    public class CustomResourceOwnerPasswordValidationContext : ResourceOwnerPasswordValidationContext
    {
        public string Code { get; set; }
    }
}
