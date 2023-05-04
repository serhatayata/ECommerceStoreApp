using IdentityServer.Api.Models.Base.Abstract;
using IdentityServer.Api.Utilities.Security.Jwt.Models;

namespace IdentityServer.Api.Models.UserModels
{
    public class UserLoginResponse
    {
        public UserLoginResponse()
        {
            
        }

        public UserLoginResponse(string userName, string verifyCode, JwtAccessToken token)
        {
            this.UserName = userName;
            this.VerifyCode = verifyCode;
            this.Token = token;

        }

        public string UserName { get; set; }
        public string VerifyCode { get; set; }
        public JwtAccessToken Token { get; set; }
    }
}
