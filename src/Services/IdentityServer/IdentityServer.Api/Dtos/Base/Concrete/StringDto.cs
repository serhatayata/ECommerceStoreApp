using IdentityServer.Api.Dtos.Base.Abstract;

namespace IdentityServer.Api.Dtos.Base.Concrete
{
    public class StringDto :IDto
    {
        public string Value { get; set; }
    }
}
