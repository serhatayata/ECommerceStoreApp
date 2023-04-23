using IdentityServer4.EntityFramework.Mappers;
using IdentityServer.Api.Data.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IdentityModel;
using System.Security.Claims;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer.Api.Entities.Identity;

namespace IdentityServer.Api.Data.SeedData
{
    public static class IdentityConfigurationDbContextSeed
    {
        public async static Task AddIdentityConfigurationSettingsAsync(IConfiguration configuration)
        {
            var services = new ServiceCollection();
            var assembly = typeof(IdentityConfigurationDbContextSeed).Assembly.GetName().Name;
            var identityConnString = configuration.GetConnectionString("DefaultConnection");

            services.AddIdentity<User, Role>(options =>
            {

            })
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();

            services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                //see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;
            })
            .AddAspNetIdentity<User>()
            .AddConfigurationStore<AppConfigurationDbContext>(options =>
            {
                options.ConfigureDbContext = b => b.UseSqlServer(identityConnString, opt => opt.MigrationsAssembly(assembly));
            }).AddOperationalStore<AppPersistedGrantDbContext>(options =>
            {
                options.ConfigureDbContext = b =>
                            b.UseSqlServer(identityConnString, opt => opt.MigrationsAssembly(assembly));
            })
            .AddDeveloperSigningCredential(); //Sertifika yoksa

            var serviceProvider = services.BuildServiceProvider();
            var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var persistedGrantDbContext = scope.ServiceProvider.GetService<AppPersistedGrantDbContext>();
            persistedGrantDbContext.Database.Migrate();

            var context = scope.ServiceProvider.GetService<AppConfigurationDbContext>();
            context.Database.Migrate();

            if (!(await context.Clients.AnyAsync()))
            {
                foreach (var client in Config.Clients)
                {
                    await context.Clients.AddAsync(client.ToEntity());
                }
                await context.SaveChangesAsync();
            }

            if (!(await context.IdentityResources.AnyAsync()))
            {
                foreach (var resource in Config.IdentityResources.ToList())
                {
                    await context.IdentityResources.AddAsync(resource.ToEntity());
                }

                await context.SaveChangesAsync();
            }

            if (!(await context.ApiScopes.AnyAsync()))
            {
                foreach (var resource in Config.ApiScopes.ToList())
                {
                    await context.ApiScopes.AddAsync(resource.ToEntity());
                }

                await context.SaveChangesAsync();
            }

            if (!(await context.ApiResources.AnyAsync()))
            {
                foreach (var resource in Config.ApiResources.ToList())
                {
                    await context.ApiResources.AddAsync(resource.ToEntity());
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
