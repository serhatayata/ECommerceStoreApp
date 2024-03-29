using AutoMapper;
using LocalizationService.Api.Attributes;
using LocalizationService.Api.Data.Contexts;
using LocalizationService.Api.Entities;
using Microsoft.Data.SqlClient;
using Polly;
using Serilog;
using System.Data.SqlTypes;
using System.Reflection;

namespace LocalizationService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 6)]
public class SeedDataServiceInstaller : IServiceInstaller
{
    public Task Install(
        IServiceCollection services, 
        IConfiguration configuration, 
        IWebHostEnvironment hostEnvironment)
    {
        var env = hostEnvironment;
        var serviceProvider = services.BuildServiceProvider();
        var context = serviceProvider.GetRequiredService<LocalizationDbContext>();
        var logger = serviceProvider.GetRequiredService<ILogger<SeedDataServiceInstaller>>();
        var mapper = serviceProvider.GetRequiredService<IMapper>();

        string rootPath = env.ContentRootPath;
        string seedFilePath = Path.Combine(rootPath, "Data", "SeedData", "SeedFiles");

        var policy = Polly.Policy.Handle<SqlException>()
                    .Or<SqlAlreadyFilledException>()
                    .Or<SqlNullValueException>()
                    .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                        logger.LogError(ex, "ERROR handling message: {ExceptionMessage} - Method : {ClassName}.{MethodName}",
                                            ex.Message,
                                            nameof(SeedDataServiceInstaller),
                                            MethodBase.GetCurrentMethod()?.Name);
                    });

        var result =  policy.ExecuteAndCapture(() =>
        {
            var languageFileList = GetAllFileLanguages();
            if (!context.Languages.Any())
            {
                logger.LogInformation("Start executing seed data : {ClassName}", nameof(Language));

                foreach (var lang in languageFileList)
                {
                    if (context.Languages.Any(l => l.Code == lang.Code))
                        continue;

                     context.Languages.Add(lang);
                }

                 context.SaveChanges();
            }

            var memberFileList = GetAllFileMembers();
            if (! context.Members.Any())
            {
                logger.LogInformation("Start executing seed data : {ClassName}", nameof(Member));

                string memberPath = Path.Combine(seedFilePath, $"{nameof(Member)}.txt");
                var memberCodeLength = configuration.GetSection("MemberCodeLength").Get<int>();

                foreach (var member in memberFileList)
                {
                    if ( context.Members.Any(m => m.Name == member.Name && m.MemberKey == member.MemberKey))
                        continue;

                     context.Members.Add(member);
                }

                 context.SaveChanges();
            }

            var resourceFileList = GetAllFileResources(context.Members.ToList());
            if (! context.Resources.Any())
            {
                logger.LogInformation("Start executing seed data : {ClassName}", nameof(Resource));

                foreach (var resource in resourceFileList)
                {
                    if ( context.Resources.Any(r => r.Tag == resource.Tag && r.ResourceCode == resource.ResourceCode))
                        continue;

                     context.Resources.Add(resource);
                }

                 context.SaveChanges();
            }
        });

        if (result != null && result.FinalException != null)
            Log.Error("ERROR handling message: {ExceptionMessage} - Method : {ClassName}.{MethodName} - Final Exception Type : {FinalExceptionType}",
                      result.FinalException.Message, nameof(SeedDataServiceInstaller),
                      MethodBase.GetCurrentMethod()?.Name,
                      result.ExceptionType);

        List<Resource> GetAllFileResources(List<Member> memberList)
        {
            string resourcePath = Path.Combine(seedFilePath, $"{nameof(Resource)}.txt");

            var resourceFileList = File.ReadAllLines(resourcePath)
                           .Skip(1)
                           .Select(m => m.Split(','))
                           .Select(m =>
                           {
                               var currentLanguage = context.Languages.FirstOrDefault(l => l.Code == m[2]);
                               var currentMember = memberList.FirstOrDefault(s => s.MemberKey == m[4]);

                               return new Resource()
                               {
                                   Tag = m[0],
                                   Value = m[1],
                                   LanguageCode = m[2],
                                   LanguageId = currentLanguage?.Id ?? 0,
                                   MemberId = currentMember?.Id ?? 0,
                                   CreateDate = DateTime.Now,
                                   ResourceCode = Guid.NewGuid().ToString(),
                                   Status = Convert.ToBoolean(m[3])
                               };
                           }).ToList();

            return resourceFileList;
        }

        List<Member> GetAllFileMembers()
        {
            string memberPath = Path.Combine(seedFilePath, $"{nameof(Member)}.txt");

            var memberFileList = File.ReadAllLines(memberPath)
                      .Skip(1)
                      .Select(m => m.Split(','))
                      .Select(m => new Member()
                      {
                          Name = m[0],
                          MemberKey = m[1],
                          CreateDate = DateTime.Now
                      }).ToList();

            return memberFileList;
        }

        List<Language> GetAllFileLanguages()
        {
            string languagePath = Path.Combine(seedFilePath, $"{nameof(Language)}.txt");

            var languageList = File.ReadAllLines(languagePath)
                                  .Skip(1)
                                  .Select(l => l.Split(','))
                                  .Select(l => new Language()
                                  {
                                      Code = l[0],
                                      DisplayName = l[1]
                                  }).ToList();

            return languageList;
        }

        return Task.CompletedTask;
    }
}
