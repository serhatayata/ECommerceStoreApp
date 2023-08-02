using ECommerceStoreApp.StateMachine.Settings;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var assembly = typeof(Program).Assembly.GetName().Name;
IWebHostEnvironment environment = builder.Environment;

#region SERVICES

builder.Services.Configure<MessageBrokerPersistenceSettings>(configuration.GetSection("MessageBroker:StateMachinePersistence"));
builder.Services.Configure<MessageBrokerQueueSettings>(configuration.GetSection("MessageBroker:QueueSettings"));


#endregion

var app = builder.Build();

#region APP
app.MapGet("/", () => "Hello World!");
#endregion

app.Run();
