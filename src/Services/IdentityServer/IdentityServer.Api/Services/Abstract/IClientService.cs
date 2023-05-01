using IdentityServer.Api.Models.Base.Concrete;
using IdentityServer.Api.Models.ClientModels;
using IdentityServer.Api.Models.IncludeOptions.Account;
using IdentityServer.Api.Services.Base;

namespace IdentityServer.Api.Services.Abstract
{
    public interface IClientService : IBaseService<ClientModel, StringModel, ClientIncludeOptions>,
                                      IAddService<ClientModel, ClientAddModel>,
                                      IDeleteService<StringModel>
    {
        
    }
}
