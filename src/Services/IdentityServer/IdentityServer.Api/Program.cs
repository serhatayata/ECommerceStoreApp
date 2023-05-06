using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using IdentityServer.Api.Data.Contexts;
using IdentityServer.Api.Data.SeedData;
using IdentityServer.Api.DependencyResolvers.Autofac;
using IdentityServer.Api.Entities.Identity;
using IdentityServer.Api.Extensions;
using IdentityServer.Api.Extensions.Authentication;
using IdentityServer.Api.Handlers;
using IdentityServer.Api.Mapping;
using IdentityServer.Api.Utilities.IoC;
using IdentityServer.Api.Validations.IdentityValidators;
using IdentityServer4;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.Hosting.LocalApiAuthentication;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

#region SERVICES
var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var assembly = typeof(Program).Assembly.GetName().Name;

builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var config = ConfigurationExtension.appConfig;
builder.Configuration.AddConfiguration(config);

#region Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});
#endregion
#region Http
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
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
#region IdentityServer
string defaultConnString = configuration.GetSection("ConnectionStrings:DefaultConnection").Value;

builder.Services.AddLogging();
builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(defaultConnString, b => b.MigrationsAssembly(assembly)), ServiceLifetime.Transient);

builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;

    options.Lockout.AllowedForNewUsers = false;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
    options.Lockout.MaxFailedAccessAttempts = 3;
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
    options.Discovery.CustomEntries.Add("update-user", "~/update-user");
    options.Discovery.CustomEntries.Add("delete-user", "~/delete-user");
})
.AddResourceOwnerValidator<ResourceOwnerPasswordCustomValidator>()
.AddProfileService<ProfileService>()
.AddAspNetIdentity<User>()
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
app.UseSession();
app.UseHttpsRedirection();
app.UseRouting();
//app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.UseIdentityServer();

app.MapControllers();
#endregion

app.Run();
