using LocalizationService.Api.Data.Repositories.Dapper.Abstract;

namespace LocalizationService.Api.Data.Repositories.Base
{
    public interface IUnitOfWork
    {
        IDapperLanguageRepository LanguageRepository { get; }
        IDapperMemberRepository MemberRepository { get; }
        IDapperResourceRepository ResourceRepository { get; }
    }
}
