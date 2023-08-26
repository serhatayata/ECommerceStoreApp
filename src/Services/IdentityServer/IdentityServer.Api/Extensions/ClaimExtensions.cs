using IdentityModel;
using IdentityServer.Api.Entities.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityServer.Api.Extensions
{
    public static class ClaimExtensions
    {
        public static void AddEmail(this ICollection<Claim> claims, string email)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, email));
        }

        public static void AddPhoneNumber(this ICollection<Claim> claims, string phoneNumber)
        {
            claims.Add(new Claim(JwtClaimTypes.PhoneNumber, phoneNumber));
        }

        public static void AddName(this ICollection<Claim> claims, string name)
        {
            claims.Add(new Claim(JwtClaimTypes.Name, name));
        }

        public static void AddNameIdentifier(this ICollection<Claim> claims, string nameIdentifier)
        {
            claims.Add(new Claim(JwtClaimTypes.Id, nameIdentifier));
        }

        public static void AddRoles(this ICollection<Claim> claims, string[] roles)
        {
            roles.ToList().ForEach(role => claims.Add(new Claim(JwtClaimTypes.Role, role)));
        }
        public static void AddUserData(this ICollection<Claim> claims, string userData)
        {
            claims.Add(new Claim(ClaimTypes.UserData, userData));
        }

        public static void AddIp(this ICollection<Claim> claims, string ip)
        {
            claims.Add(new Claim(JwtClaimTypes.Locale, ip));
        }


        public static Claim[] GetUserClaims(User user, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Id, user.Id),
                new Claim(JwtClaimTypes.Name, (!string.IsNullOrEmpty(user.Name) && !string.IsNullOrEmpty(user.Surname)) ? (user.Name + " " + user.Surname) : string.Empty),
                new Claim(JwtClaimTypes.GivenName, user.Name  ?? string.Empty),
                new Claim(JwtClaimTypes.FamilyName, user.Surname  ?? string.Empty),
                new Claim(JwtClaimTypes.Email, user.Email  ?? string.Empty),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, role));
                claims.Add(new Claim("Roles", role));
            }

            return claims.ToArray();
        }

        public static Claim[] GetClaims(List<string> claims)
        {
            var result = new List<Claim>();

            foreach (var role in claims)
                result.Add(new Claim(JwtClaimTypes.Role, role));

            return result.ToArray();
        }
    }
}
