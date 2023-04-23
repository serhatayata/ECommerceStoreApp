using IdentityServer.Api.Data.SeedData;
using IdentityServer.Api.Extensions;
using System.Reflection;

#region SERVICES
var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var assembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

builder.Services.AddControllers();

var config = ConfigurationExtension.appConfig;
builder.Configuration.AddConfiguration(config);

#region IdentityServer
string defaultConnString = configuration.GetSection("ConnectionStrings:DefaultConnection").Value;

await IdentityDbContextSeed.AddUserSeedAsync(configuration, defaultConnString, assembly);
await IdentityConfigurationDbContextSeed.AddIdentityConfigurationSeedAsync(configuration, defaultConnString, assembly);
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

app.UseHttpsRedirection();
app.UseRouting();
//app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.UseIdentityServer();

app.MapControllers();
#endregion

app.Run();
