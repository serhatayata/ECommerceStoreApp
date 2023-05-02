using IdentityServer.Api.Models.Base.Abstract;

namespace IdentityServer.Api.Models.UserModels
{
    public class UserLoginResponse
    {
        public string UserName { get; set; }
        public string Code { get; set; }
    }
}
