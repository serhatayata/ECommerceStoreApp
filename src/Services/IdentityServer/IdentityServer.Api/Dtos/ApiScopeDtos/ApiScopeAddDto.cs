using IdentityServer.Api.Dtos.Base.Abstract;
using IdentityServer.Api.Dtos.Base.Concrete;
using IdentityServer4.EntityFramework.Entities;

namespace IdentityServer.Api.Dtos.ApiScopeDtos
{
    public class ApiScopeAddDto : IAddDto
    {
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        public List<string> UserClaims { get; set; }
        public List<PropertyDto> Properties { get; set; }
    }
}
