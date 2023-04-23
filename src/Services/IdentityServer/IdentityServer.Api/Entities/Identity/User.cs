using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Api.Entities.Identity
{
    public class User : IdentityUser<string>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CountryCode { get; set; }
        public byte Status { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime LastSeen { get; set; }
    }

    public enum UserStatus : byte
    {
        NotValidated = 1,
        Validated = 2,
        Suspended = 3
    }
}
