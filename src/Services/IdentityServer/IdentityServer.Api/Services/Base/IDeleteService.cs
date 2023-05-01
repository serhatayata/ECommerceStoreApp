﻿using IdentityServer.Api.Models.Base.Abstract;
using IdentityServer.Api.Utilities.Results;

namespace IdentityServer.Api.Services.Base
{
    public interface IDeleteService<D> where D : class, IDeleteModel
    {
        /// <summary>
        /// Delete by <typeparamref name="D"/>
        /// </summary>
        /// <param name="id"><typeparamref name="D"/>Delete model</param>
        /// <returns><see cref="Result"/></returns>
        Result Delete(D model);
    }
}
