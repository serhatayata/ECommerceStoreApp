namespace IdentityServer.Api.Utilities.Security.Jwt.Models
{
    public class JwtAccessToken
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
