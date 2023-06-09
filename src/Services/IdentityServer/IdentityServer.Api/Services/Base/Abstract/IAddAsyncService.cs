﻿using IdentityServer.Api.Models.Base.Abstract;
using IdentityServer.Api.Utilities.Results;

namespace IdentityServer.Api.Services.Base.Abstract
{
    public interface IAddAsyncService<T, A> where A : class, IAddModel
                                            where T : class, IModel
    {
        /// <summary>
        /// Add <typeparamref name="A"/>
        /// </summary>
        /// <param name="model"><see cref="A"/></param>
        /// <returns><typeparamref name="T"/></returns>
        Task<DataResult<T>> AddAsync(A model);
    }
}
