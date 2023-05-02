using IdentityServer.Api.Models.Base.Abstract;

namespace IdentityServer.Api.Models.UserModels
{
    public class UserLoginModel : IModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
