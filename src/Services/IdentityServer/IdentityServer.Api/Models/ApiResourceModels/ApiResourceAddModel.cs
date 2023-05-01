using IdentityServer.Api.Models.Base.Abstract;
using IdentityServer.Api.Models.Base.Concrete;
using IdentityServer4.EntityFramework.Entities;

namespace IdentityServer.Api.Models.ApiResourceModels
{
    public class ApiResourceAddModel : IAddModel
    {
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public List<string> AllowedAccessTokenSigningAlgorithms { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        public List<IdentityServer4.Models.Secret> Secrets { get; set; }
        public List<string> Scopes { get; set; }
        public List<string> UserClaims { get; set; }
        public List<PropertyModel> Properties { get; set; }
        public bool NonEditable { get; set; }
    }
}
