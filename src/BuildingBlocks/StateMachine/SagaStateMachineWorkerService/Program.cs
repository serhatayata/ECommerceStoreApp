using MassTransit;
using Microsoft.EntityFrameworkCore;
using SagaStateMachineWorkerService;
using SagaStateMachineWorkerService.Extensions;
using SagaStateMachineWorkerService.Infrastructure.Contexts;
using SagaStateMachineWorkerService.Models;
using Shared.Queue.Events;
using System.Reflection;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddMassTransit(m =>
        {
            m.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>()
             .EntityFrameworkRepository(opt =>
             {
                 opt.AddDbContext<DbContext, OrderStateDbContext>((srv, cfg) =>
                 {
                     cfg.UseSqlServer(connectionString: hostContext.Configuration.GetConnectionString("SqlConnection"),
                                      sqlServerOptionsAction: sqlOpt =>
                                      {
                                          sqlOpt.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                                      });
                 });
             });

            var envName = hostContext.HostingEnvironment.EnvironmentName;

            m.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMQHost = hostContext.Configuration.GetConnectionString("RabbitMQ");
                cfg.Host(rabbitMQHost);

                //Order Initial
                var orderCreatedRequestEventName = MessageBrokerExtensions.GetQueueName<OrderCreatedRequestEvent>();
                cfg.ReceiveEndpoint(orderCreatedRequestEventName, e =>
                {
                    // When OrderSaga queue get any message OrderStateInstance will be created as a record in DB
                    e.ConfigureSaga<OrderStateInstance>(context);
                });
            });
        });

        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
