using IdentityModel;
using IdentityServer.Api.Data.Contexts;
using IdentityServer.Api.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IdentityServer.Api.Data.SeedData
{
    public class IdentityUserContextSeed
    {
        public async static Task AddUserSettingsAsync(string connString)
        {
            var assembly = typeof(Program).Assembly.GetName().Name;
            var services = new ServiceCollection();
            services.AddLogging();

            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(connString, b => b.MigrationsAssembly(assembly)));

            services.AddIdentity<User, Role>(options =>
            {

            })
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();

            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var context = scope.ServiceProvider.GetService<AppIdentityDbContext>();
            context.Database.Migrate();

            #region User_1
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var user_1 = await userMgr.FindByNameAsync("907777777777");
            if (user_1 == null)
            {
                user_1 = new User
                {
                    Name = "Serhat",
                    Surname = "Ayata",
                    CreateTime = DateTime.Now,
                    LastSeen = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    CountryCode = "TR",
                    PhoneNumber = "7777777777",
                    UserName = "907777777777",
                    Status = (byte)UserStatus.NotValidated,
                    Email = "srht1@email.com",
                    EmailConfirmed = true,
                };
                var result = await userMgr.CreateAsync(user_1, "Password12*");
                if (!result.Succeeded)
                    throw new Exception(result.Errors.First().Description);

                result = await userMgr.AddClaimsAsync(user_1, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Serhat Ayata"),
                            new Claim(JwtClaimTypes.GivenName, "Serhat"),
                            new Claim(JwtClaimTypes.FamilyName, "Serhat"),
                            new Claim(JwtClaimTypes.WebSite, "http://serhatayata.com"),
                        });

                if (!result.Succeeded)
                    throw new Exception(result.Errors.First().Description);
            }
            else
            {

            }
            #endregion
            #region User_2
            var user_2 = await userMgr.FindByNameAsync("905555555555");
            if (user_2 == null)
            {
                user_2 = new User
                {
                    Name = "Mehmet",
                    Surname = "Kaya",
                    CreateTime = DateTime.Now,
                    LastSeen = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    CountryCode = "TR",
                    PhoneNumber = "5555555555",
                    UserName = "905555555555",
                    Status = (byte)UserStatus.NotValidated,
                    Email = "mkaya@email.com",
                    EmailConfirmed = true,
                };
                var result = await userMgr.CreateAsync(user_2, "Password12*");
                if (!result.Succeeded)
                    throw new Exception(result.Errors.First().Description);

                result = await userMgr.AddClaimsAsync(user_2, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Mehmet Kaya"),
                            new Claim(JwtClaimTypes.GivenName, "Mehmet"),
                            new Claim(JwtClaimTypes.FamilyName, "Kaya"),
                            new Claim(JwtClaimTypes.WebSite, "http://blabla.com"),
                            new Claim("location", "somewhere")
                        });
                if (!result.Succeeded)
                    throw new Exception(result.Errors.First().Description);
            }
            else
            {

            }
            #endregion
            #region Roles
            var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

            var role_1 = await roleMgr.FindByNameAsync("User.Normal");
            if (role_1 == null)
            {
                role_1 = new Role()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "User.Normal",
                    NormalizedName = "USER.NORMAL"
                };

                var result = await roleMgr.CreateAsync(role_1);
                if (!result.Succeeded)
                    throw new Exception(result.Errors.First().Description);
            }

            var role_2 = await roleMgr.FindByNameAsync("User.Admin");
            if (role_2 == null)
            {
                role_2 = new Role()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "User.Admin",
                    NormalizedName = "USER.ADMIN"
                };

                var result = await roleMgr.CreateAsync(role_2);
                if (!result.Succeeded)
                    throw new Exception(result.Errors.First().Description);
            }
            #endregion
            #region UserRoles
            await userMgr.AddToRoleAsync(user_1, "User.Admin");
            await userMgr.AddToRoleAsync(user_2, "User.Normal");
            #endregion
        }
    }
}
