using IdentityServer4.EntityFramework.Mappers;
using IdentityServer.Api.Data.Contexts;
using IdentityServer.Api.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Api.Data.SeedData
{
    public class IdentityConfigurationDbContextSeed
    {
        public async static Task AddIdentityConfigurationSeedAsync(IConfiguration configuration, string connectionString, string assembly)
        {
            var services = new ServiceCollection();

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
                options.EmitStaticAudienceClaim = true;
            })
            .AddAspNetIdentity<User>()
            .AddConfigurationStore<AppConfigurationDbContext>(options =>
            {
                options.ConfigureDbContext = b => b.UseSqlServer(connectionString, opt => opt.MigrationsAssembly(assembly));
            }).AddOperationalStore<AppPersistedGrantDbContext>(options =>
            {
                options.ConfigureDbContext = b =>
                            b.UseSqlServer(connectionString, opt => opt.MigrationsAssembly(assembly));
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
