using LocalizationService.Api.Data.Repositories.Dapper.Abstract;
using LocalizationService.Api.Data.Repositories.EntityFramework.Abstract;

namespace LocalizationService.Api.Data.Repositories.Base
{
    public interface IUnitOfWork
    {
        IDapperLanguageRepository LanguageRepository { get; }
        IDapperMemberRepository MemberRepository { get; }
        IDapperResourceRepository ResourceRepository { get; }

        IEfLanguageRepository EfLanguageRepository { get; }
        IEfMemberRepository EfMemberRepository { get; }
        IEfResourceRepository EfResourceRepository { get; }
    }
}
