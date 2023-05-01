using IdentityServer.Api.Models.Base.Abstract;

namespace IdentityServer.Api.Models.Base.Concrete
{
    public class IntModel : IModel, IDeleteModel
    {
        public int Id { get; set; }
    }
}
