using IdentityServer.Api.Models.Base.Abstract;
using IdentityServer.Api.Models.Base.Concrete;
using IdentityServer4.EntityFramework.Entities;

namespace IdentityServer.Api.Models.ApiScopeModels
{
    public class ApiScopeAddModel : IAddModel
    {
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        public List<string> UserClaims { get; set; }
        public List<PropertyModel> Properties { get; set; }
    }
}
