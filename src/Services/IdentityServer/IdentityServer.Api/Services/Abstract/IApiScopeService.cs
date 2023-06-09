﻿using IdentityServer.Api.Models.ApiScopeModels;
using IdentityServer.Api.Models.Base.Concrete;
using IdentityServer.Api.Models.IncludeOptions.Account;
using IdentityServer.Api.Services.Base.Abstract;
using IdentityServer.Api.Utilities.Results;

namespace IdentityServer.Api.Services.Abstract
{
    public interface IApiScopeService : IBaseService<ApiScopeModel, StringModel, ApiScopeIncludeOptions>,
                                        IAddService<ApiScopeModel, ApiScopeAddModel>,
                                        IDeleteService<StringModel>
    {
        DataResult<List<ApiScopeModel>> Get(List<string> apiScopeNames, ApiScopeIncludeOptions options);
    }
}
