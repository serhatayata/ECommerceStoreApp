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
        public async static Task AddIdentityConfigurationSettingsAsync(AppConfigurationDbContext context, AppPersistedGrantDbContext persistedGrantDbContext)
        {
            persistedGrantDbContext.Database.Migrate();
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
