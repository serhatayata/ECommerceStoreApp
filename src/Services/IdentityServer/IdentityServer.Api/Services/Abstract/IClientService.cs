using IdentityServer.Api.Dtos;
using IdentityServer.Api.Dtos.Base.Concrete;
using IdentityServer.Api.Dtos.ClientDtos;
using IdentityServer.Api.Models.IncludeOptions.Account;
using IdentityServer.Api.Services.Base;

namespace IdentityServer.Api.Services.Abstract
{
    public interface IClientService : IBaseService<ClientDto, StringDto, ClientIncludeOptions>,
                                      IAddService<ClientDto, ClientAddDto>,
                                      IDeleteService<StringDto>
    {
        
    }
}
