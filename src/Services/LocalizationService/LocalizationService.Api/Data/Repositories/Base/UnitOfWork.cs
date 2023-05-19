using LocalizationService.Api.Data.Repositories.Dapper.Abstract;

namespace LocalizationService.Api.Data.Repositories.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IDapperLanguageRepository languageRepository, 
                          IDapperMemberRepository memberRepository, 
                          IDapperResourceRepository resourceRepository)
        {
            LanguageRepository = languageRepository;
            MemberRepository = memberRepository;
            ResourceRepository = resourceRepository;
        }

        public IDapperLanguageRepository LanguageRepository { get;}

        public IDapperMemberRepository MemberRepository { get; }

        public IDapperResourceRepository ResourceRepository { get; }
    }
}
