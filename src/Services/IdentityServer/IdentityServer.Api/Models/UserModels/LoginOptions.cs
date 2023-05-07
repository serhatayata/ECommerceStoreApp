namespace IdentityServer.Api.Models.UserModels
{
    public class LoginOptions
    {
        public int DatabaseId { get; set; }
        public string Prefix { get; set; }
        public int VerifyCodeDuration { get; set; }
        public string VerifyCodeRole { get; set; }
        public int LockoutTimeSpan { get; set; }
        public int MaxFailedAccessAttempts { get; set; }
    }
}
