using AutoMapper;
using LocalizationService.Api.Data.Contexts;
using LocalizationService.Api.Entities;
using LocalizationService.Api.Extensions;
using LocalizationService.Api.Models.ResourceModels;
using LocalizationService.Api.Services.Redis.Abstract;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;
using System.Data.SqlTypes;
using System.Reflection;

namespace LocalizationService.Api.Data.SeedData;

public class LocalizationSeedData
{
    public async static Task LoadLocalizationSeedDataAsync(LocalizationDbContext context, IServiceScope scope, IWebHostEnvironment env, IConfiguration configuration)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<LocalizationSeedData>>();
        var redisService = scope.ServiceProvider.GetRequiredService<IRedisService>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

        string rootPath = env.ContentRootPath;
        string seedFilePath = Path.Combine(rootPath, "Data", "SeedData", "SeedFiles");

        var policy = Polly.Policy.Handle<SqlException>()
                    .Or<SqlAlreadyFilledException>()
                    .Or<SqlNullValueException>()
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                        logger.LogError(ex, "ERROR handling message: {ExceptionMessage} - Method : {ClassName}.{MethodName}",
                                            ex.Message, 
                                            nameof(LocalizationSeedData),
                                            MethodBase.GetCurrentMethod()?.Name);
                    });

        await policy.ExecuteAsync(async () =>
        {
            await context.Database.MigrateAsync();

            var languageFileList = GetAllFileLanguages();
            if (!await context.Languages.AnyAsync())
            {
                logger.LogInformation("Start executing seed data : {ClassName}", nameof(Language));

                foreach (var lang in languageFileList)
                {
                    if (await context.Languages.AnyAsync(l => l.Code == lang.Code))
                        continue;

                    await context.Languages.AddAsync(lang);
                }

                await context.SaveChangesAsync();
            }

            var memberFileList = GetAllFileMembers();
            if (!await context.Members.AnyAsync())
            {
                logger.LogInformation("Start executing seed data : {ClassName}", nameof(Member));

                string memberPath = Path.Combine(seedFilePath, $"{nameof(Member)}.txt");
                var memberCodeLength = configuration.GetSection("MemberCodeLength").Get<int>();

                foreach (var member in memberFileList)
                {
                    if (await context.Members.AnyAsync(m => m.Name == member.Name && m.MemberKey == member.MemberKey))
                        continue;

                    await context.Members.AddAsync(member);
                }

                await context.SaveChangesAsync();
            }

            var resourceFileList = GetAllFileResources(context.Members.ToList());
            if (!await context.Resources.AnyAsync())
            {
                logger.LogInformation("Start executing seed data : {ClassName}", nameof(Resource));

                foreach (var resource in resourceFileList)
                {
                    if (await context.Resources.AnyAsync(r => r.Tag == resource.Tag && r.ResourceCode == resource.ResourceCode))
                        continue;

                    await context.Resources.AddAsync(resource);
                }

                await context.SaveChangesAsync();
            }

            var cacheResourceList = mapper.Map<List<ResourceCacheModel>>(resourceFileList);
            foreach (var resourceData in cacheResourceList)
            {
                //Cache
                var currentMember = context.Members.FirstOrDefault(s => s.Id == resourceData.MemberId);
                if (currentMember == null)
                    continue;

                var duration = configuration.GetSection("LocalizationCacheSettings:Duration").Get<int>();
                var databaseId = configuration.GetSection("LocalizationCacheSettings:DatabaseId").Get<int>();
                var cacheKey = CacheExtensions.GetResourceCacheKey(currentMember.MemberKey, resourceData.LanguageCode, resourceData.Tag);
                await redisService.SetAsync(cacheKey, resourceData, duration, databaseId);
            }
        });

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
    }
}
