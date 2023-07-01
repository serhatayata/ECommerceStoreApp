using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;

namespace CatalogService.Api.Data.Repositories.Base;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(IDapperLanguageRepository languageRepository, 
                      IDapperMemberRepository memberRepository, 
                      IDapperResourceRepository resourceRepository,
                      IEfLanguageRepository efLanguageRepository,
                      IEfMemberRepository efMemberRepository,
                      IEfResourceRepository efResourceRepository)
    {
        LanguageRepository = languageRepository;
        MemberRepository = memberRepository;
        ResourceRepository = resourceRepository;
        EfLanguageRepository = efLanguageRepository;
        EfMemberRepository = efMemberRepository;
        EfResourceRepository = efResourceRepository;
    }

    public IDapperLanguageRepository LanguageRepository { get;}

    public IDapperMemberRepository MemberRepository { get; }

    public IDapperResourceRepository ResourceRepository { get; }


    public IEfLanguageRepository EfLanguageRepository { get; }

    public IEfMemberRepository EfMemberRepository { get; }

    public IEfResourceRepository EfResourceRepository { get; }
}
