using Autofac;
using Autofac.Extensions.DependencyInjection;
using IdentityServer.Api.CacheStores;
using IdentityServer.Api.Data.Contexts;
using IdentityServer.Api.Data.SeedData;
using IdentityServer.Api.DependencyResolvers.Autofac;
using IdentityServer.Api.Entities.Identity;
using IdentityServer.Api.Extensions;
using IdentityServer.Api.Extensions.Authentication;
using IdentityServer.Api.Extensions.MiddlewareExtensions;
using IdentityServer.Api.Mapping;
using IdentityServer.Api.Models.LogModels;
using IdentityServer.Api.Models.Settings;
using IdentityServer.Api.Models.UserModels;
using IdentityServer.Api.Services.Base.Concrete;
using IdentityServer.Api.Services.ElasticSearch.Abstract;
using IdentityServer.Api.Services.ElasticSearch.Concrete;
using IdentityServer.Api.Services.Redis.Abstract;
using IdentityServer.Api.Services.Redis.Concrete;
using IdentityServer.Api.Services.Token.Abstract;
using IdentityServer.Api.Services.Token.Concrete;
using IdentityServer.Api.Utilities.IoC;
using IdentityServer.Api.Utilities.Security.Jwt;
using IdentityServer.Api.Validations.IdentityValidators;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

#region SERVICES
var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var assembly = typeof(Program).Assembly.GetName().Name;
IWebHostEnvironment environment = builder.Environment;

builder.Services.AddControllerSettings();

var config = ConfigurationExtension.appConfig;
var serilogConfig = ConfigurationExtension.serilogConfig;

builder.Host.AddHostExtensions(environment);
builder.Services.AddElasticSearchConfiguration();

#region Startup DI
builder.Services.AddSingleton<IElasticSearchService, ElasticSearchService>();
builder.Services.AddSingleton<IRedisService, RedisService>();
builder.Services.AddSingleton<IJwtHelper, JwtHelper>();

builder.Services.AddTransient<IClientCredentialsTokenService, ClientCredentialsTokenService>();
#endregion
#region MemoryCache
builder.Services.AddMemoryCache();
#endregion
#region Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});
#endregion
#region Configure
builder.Services.Configure<SourceOrigin>(configuration.GetSection($"SourceOriginSettings:{environment.EnvironmentName}"));
#endregion
#region Http
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpClients(configuration);

builder.Services.AddAccessTokenManagement();
#endregion
#region Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacBusinessModule()));
#endregion
#region AutoMapper
builder.Services.AddAutoMapper(typeof(MapProfile).Assembly);
#endregion
#region Logging
builder.Services.AddLogging();
#endregion
#region Configuration Settings
builder.Configuration.AddConfiguration(config);
#endregion
#region IdentityServer
string defaultConnString = configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
var loginOptions = configuration.GetSection("LoginOptions").Get<LoginOptions>();

builder.Services.AddLogging();
builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(defaultConnString, b => b.MigrationsAssembly(assembly)), ServiceLifetime.Transient);

builder.Services.AddIdentity<User, Role>(options =>
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

builder.Services.AddIdentityServer(options =>
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

var serviceProvider = builder.Services.BuildServiceProvider();
var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

var identityDbContext = scope.ServiceProvider.GetService<AppIdentityDbContext>();
var persistedGrantDbContext = scope.ServiceProvider.GetService<AppPersistedGrantDbContext>();
var configurationContext = scope.ServiceProvider.GetService<AppConfigurationDbContext>();

await IdentityUserContextSeed.AddUserSettingsAsync(identityDbContext, scope);
await IdentityConfigurationDbContextSeed.AddIdentityConfigurationSettingsAsync(configurationContext, persistedGrantDbContext);
#endregion
#region ElasticSearch
var elasticSearchService = scope.ServiceProvider.GetRequiredService<IElasticSearchService>();

var elasticLogOptions = configuration.GetSection("ElasticSearchOptions").Get<ElasticSearchOptions>();
await elasticSearchService.CreateIndexAsync<LogDetail>(elasticLogOptions.LogIndex);
#endregion
#region Authentication - Authorization
string verifyCodeRole = configuration.GetValue<string>("Schemes:VerifyCode");

builder.Services.AddLocalApiAuthentication();
builder.Services.UseVerifyCodeTokenAuthentication();

//builder.Services.AddAuthentication(option =>
//{
//    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//  .AddJwtBearer(AuthenticationSchemeConstants.VerifyCode, options =>
//  {
//      options.TokenValidationParameters = new TokenValidationParameters
//      {
//          ValidateIssuer = true,
//          ValidateAudience = true,
//          ValidateIssuerSigningKey = true,
//          ValidIssuer = configuration.GetValue<string>("JwtTokenOptionsForVerify:Issuer"),
//          ValidAudience = configuration.GetValue<string>("JwtTokenOptionsForVerify:Audience"),
//          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JwtTokenOptionsForVerify:SecretKey")))
//      };
//  });

//builder.Services.AddAuthorization(options =>
//{
//    var onlySecondJwtSchemePolicyBuilder = new AuthorizationPolicyBuilder(AuthenticationPolicyConstants.VerifyCodePolicy);
//    options.AddPolicy(AuthenticationPolicyConstants.VerifyCodePolicy, onlySecondJwtSchemePolicyBuilder
//        .RequireAuthenticatedUser()
//        .Build());
//});

#endregion
#region Localization
builder.Services.AddHostedService<InitializeCacheService>();

await builder.Services.AddLocalizationSettingsAsync(configuration);
#endregion

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

var app = builder.Build();

#region PIPELINE
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureCustomExceptionMiddleware();
app.UseStaticFiles();
app.UseSession();
app.UseHttpsRedirection();
app.UseRouting();
//app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.UseLocalizationMiddleware();
app.UseIdentityServer();

app.MapControllers();
#endregion

app.Run();
