using IdentityServer.Api.Dtos.Base.Abstract;

namespace IdentityServer.Api.Dtos.Base.Concrete
{
    public class PropertyDto : IDto
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
