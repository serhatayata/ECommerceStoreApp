using LocalizationService.Api.Data.Contexts;
using LocalizationService.Api.Entities;
using LocalizationService.Api.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;
using System.Data.SqlTypes;
using System.Reflection;

namespace LocalizationService.Api.Data.SeedData
{
    public class LocalizationSeedData
    {
        public async static Task LoadLocalizationSeedDataAsync(LocalizationDbContext context, IServiceScope scope, IWebHostEnvironment env, IConfiguration configuration)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<LocalizationSeedData>>();

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

                if (!await context.Languages.AnyAsync())
                {
                    logger.LogInformation("Start executing seed data : {ClassName}", nameof(Language));

                    string languagePath = Path.Combine(seedFilePath, $"{nameof(Language)}.txt");

                    var languageList = File.ReadAllLines(languagePath)
                                          .Skip(1)
                                          .Select(l => l.Split(','))
                                          .Select(l => new Language()
                                          {
                                              Code = l[0],
                                              DisplayName = l[1]
                                          }).ToList();

                    foreach (var lang in languageList)
                    {
                        if (await context.Languages.AnyAsync(l => l.Code == lang.Code))
                            continue;

                        await context.Languages.AddAsync(lang);
                        await context.SaveChangesAsync();
                    }
                }

                if (!await context.Members.AnyAsync())
                {
                    logger.LogInformation("Start executing seed data : {ClassName}", nameof(Member));

                    string memberPath = Path.Combine(seedFilePath, $"{nameof(Member)}.txt");

                    var memberCodeLength = configuration.GetSection("MemberCodeLength").Get<int>();

                    var memberList = File.ReadAllLines(memberPath)
                                          .Skip(1)
                                          .Select(m => m.Split(','))
                                          .Select(m => new Member()
                                          {
                                              Name = m[0],
                                              MemberKey = m[1],
                                              LocalizationPrefix = m[2],
                                              //MemberKey = RandomExtensions.RandomCode(memberCodeLength),
                                              CreateDate = DateTime.Now
                                          }).ToList();

                    foreach (var member in memberList)
                    {
                        if (await context.Members.AnyAsync(m => m.Name == member.Name && m.MemberKey == member.MemberKey))
                            continue;

                        await context.Members.AddAsync(member);
                        await context.SaveChangesAsync();
                    }
                }

                if (!await context.Resources.AnyAsync())
                {
                    logger.LogInformation("Start executing seed data : {ClassName}", nameof(Resource));

                    string resourcePath = Path.Combine(seedFilePath, $"{nameof(Resource)}.txt");
                    var memberList = await context.Members.ToListAsync();

                    var resourceList = File.ReadAllLines(resourcePath)
                                           .Skip(1)
                                           .Select(m => m.Split(','))
                                           .Select(m =>
                                           {
                                               var currentLanguage = context.Languages.FirstOrDefault(l => l.Code == m[2]);
                                               var rnd = new Random();

                                               var currentMember = memberList.ElementAtOrDefault(rnd.Next(0, 3));
                                               if (memberList.Count() > 0 && currentMember == null)
                                               {
                                                   while(currentMember == null)
                                                   {
                                                       currentMember = memberList.ElementAtOrDefault(rnd.Next(0, 3));
                                                   }
                                               }

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

                    foreach (var resource in resourceList)
                    {
                        if (await context.Resources.AnyAsync(r => r.Tag == resource.Tag && r.ResourceCode == resource.ResourceCode))
                            continue;

                        await context.Resources.AddAsync(resource);
                        await context.SaveChangesAsync();
                    }
                }
            });
        }
    }
}
