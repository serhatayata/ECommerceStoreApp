using IdentityServer.Api.Attributes;
using IdentityServer.Api.CacheStores;
using IdentityServer.Api.Data.Contexts;
using IdentityServer.Api.Data.SeedData;
using IdentityServer.Api.Entities.Identity;
using IdentityServer.Api.Models.UserModels;
using IdentityServer.Api.Validations.IdentityValidators;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 12)]
public class IdentityServiceInstaller : IServiceInstaller
{
    public async void Install(
        IServiceCollection services, 
        IConfiguration configuration, 
        IWebHostEnvironment hostEnvironment)
    {
        var assembly = typeof(Program).Assembly.GetName().Name;

        string defaultConnString = configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
        var loginOptions = configuration.GetSection("LoginOptions").Get<LoginOptions>();

        services.AddIdentity<User, Role>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;

            options.Lockout.AllowedForNewUsers = false;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(loginOptions.LockoutTimeSpan);
            options.Lockout.MaxFailedAccessAttempts = loginOptions.MaxFailedAccessAttempts;
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

            options.Caching.ClientStoreExpiration = TimeSpan.FromDays(1);
            options.Caching.ResourceStoreExpiration = TimeSpan.FromDays(1);

            options.Discovery.CustomEntries.Add("update-user", "~/update-user");
            options.Discovery.CustomEntries.Add("delete-user", "~/delete-user");
        })
        .AddResourceOwnerValidator<ResourceOwnerPasswordCustomValidator>()
        .AddProfileService<ProfileService>()
        .AddAspNetIdentity<User>()
        .AddClientStoreCache<CustomClientStore>()
        .AddResourceStoreCache<CustomResourceStore>()
        .AddConfigurationStore<AppConfigurationDbContext>(options =>
        {
            options.ConfigureDbContext = b => b.UseSqlServer(defaultConnString, opt => opt.MigrationsAssembly(assembly));
        }).AddOperationalStore<AppPersistedGrantDbContext>(options =>
        {
            options.ConfigureDbContext = b =>
                        b.UseSqlServer(defaultConnString, opt => opt.MigrationsAssembly(assembly));
        })
        .AddDeveloperSigningCredential(); //Sertifika yoksa

        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

        var identityDbContext = scope.ServiceProvider.GetService<AppIdentityDbContext>();
        var persistedGrantDbContext = scope.ServiceProvider.GetService<AppPersistedGrantDbContext>();
        var configurationContext = scope.ServiceProvider.GetService<AppConfigurationDbContext>();

        await IdentityUserContextSeed.AddUserSettingsAsync(identityDbContext, scope);
        await IdentityConfigurationDbContextSeed.AddIdentityConfigurationSettingsAsync(configurationContext, persistedGrantDbContext);
    }
}
