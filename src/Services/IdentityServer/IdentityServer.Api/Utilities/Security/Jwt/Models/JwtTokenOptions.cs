namespace IdentityServer.Api.Utilities.Security.Jwt.Models
{
    public class JwtTokenOptions
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string SecretKey { get; set; }
        public int AccessTokenExpiration { get; set; }
        public int RefreshTokenExpiration { get; set; }

    }
}
