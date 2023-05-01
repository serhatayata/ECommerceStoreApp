using IdentityServer.Api.Dtos.Base.Abstract;

namespace IdentityServer.Api.Dtos.Base.Concrete
{
    public class IntDto : IDto, IDeleteDto
    {
        public int Id { get; set; }
    }
}
