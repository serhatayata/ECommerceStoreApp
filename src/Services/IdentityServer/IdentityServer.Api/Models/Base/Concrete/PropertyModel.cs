using IdentityServer.Api.Models.Base.Abstract;

namespace IdentityServer.Api.Models.Base.Concrete
{
    public class PropertyModel : IModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
