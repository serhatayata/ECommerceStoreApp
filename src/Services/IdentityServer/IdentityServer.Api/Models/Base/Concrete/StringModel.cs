using IdentityServer.Api.Models.Base.Abstract;

namespace IdentityServer.Api.Models.Base.Concrete
{
    public class StringModel : IModel, IDeleteModel
    {
        public string Value { get; set; }
    }
}
