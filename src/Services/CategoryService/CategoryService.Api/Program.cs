using CategoryService.Api.Extensions;
using CategoryService.Api.Infrastructure;
using Serilog;

var appConfig = ConfigurationExtension.appConfig;
var serilogConf = ConfigurationExtension.serilogConfig;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
{
    Args = args,
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory(),
    WebRootPath = "wwwroot"
});
ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

// app configuration
builder.Configuration.AddConfiguration(appConfig);

builder.Logging.ClearProviders(); // Remove all added providers before
builder.Host.UseSerilog();

builder.Host.UseDefaultServiceProvider((context, options) =>
{
    options.ValidateOnBuild = false;
    options.ValidateScopes = false;
});

#region SERVICES
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Configure
builder.Services.Configure<CatalogSettings>(configuration.GetSection("CatalogSettings"));
#endregion

builder.Services.ConfigureConsul(configuration);

#region DbContext
builder.Services.ConfigureDbContext(configuration);
#endregion
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
