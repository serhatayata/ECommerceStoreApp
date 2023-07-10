using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;

namespace CatalogService.Api.Data.Repositories.Base;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(IDapperBrandRepository dapperBrandRepository, 
                      IDapperCategoryRepository dapperCategoryRepository, 
                      IDapperCommentRepository dapperCommentRepository, 
                      IDapperFeatureRepository dapperFeatureRepository, 
                      IDapperProductRepository dapperProductRepository, 
                      IEfBrandRepository efBrandRepository, 
                      IEfCategoryRepository efCategoryRepository, 
                      IEfCommentRepository efCommentRepository, 
                      IEfFeatureRepository efFeatureRepository, 
                      IEfProductRepository efProductRepository)
    {
        DapperBrandRepository = dapperBrandRepository;
        DapperCategoryRepository = dapperCategoryRepository;
        DapperCommentRepository = dapperCommentRepository;
        DapperFeatureRepository = dapperFeatureRepository;
        DapperProductRepository = dapperProductRepository;

        EfBrandRepository = efBrandRepository;
        EfCategoryRepository = efCategoryRepository;
        EfCommentRepository = efCommentRepository;
        EfFeatureRepository = efFeatureRepository;
        EfProductRepository = efProductRepository;
    }

    public IDapperBrandRepository DapperBrandRepository { get; }

    public IDapperCategoryRepository DapperCategoryRepository { get; }

    public IDapperCommentRepository DapperCommentRepository { get; }

    public IDapperFeatureRepository DapperFeatureRepository { get; }

    public IDapperProductRepository DapperProductRepository { get; }

    public IEfBrandRepository EfBrandRepository { get; }

    public IEfCategoryRepository EfCategoryRepository { get; }

    public IEfCommentRepository EfCommentRepository { get; }

    public IEfFeatureRepository EfFeatureRepository { get; }

    public IEfProductRepository EfProductRepository { get; }
}
