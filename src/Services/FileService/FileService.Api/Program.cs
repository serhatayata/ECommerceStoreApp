using FileService.Api.Extensions;

var appConfig = ConfigurationExtension.appConfig;
var serilogConf = ConfigurationExtension.serilogConfig;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddConfiguration(appConfig);
builder.Host.UseDefaultServiceProvider((context, options) =>
{
    options.ValidateOnBuild = false;
    options.ValidateScopes = false;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
