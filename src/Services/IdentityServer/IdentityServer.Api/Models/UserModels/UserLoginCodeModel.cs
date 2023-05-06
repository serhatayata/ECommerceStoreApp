namespace IdentityServer.Api.Models.UserModels
{
    public class UserLoginCodeModel
    {
        public string UserName { get; set; }
        public string VerifyCode { get; set; }
    }
}
