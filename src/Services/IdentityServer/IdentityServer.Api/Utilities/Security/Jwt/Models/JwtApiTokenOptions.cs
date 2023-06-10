namespace IdentityServer.Api.Utilities.Security.Jwt.Models
{
    public class JwtApiTokenOptions
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string ClientId { get; set; }
        public string SecretKey { get; set; }
        public int AccessTokenExpiration { get; set; }
    }
}
