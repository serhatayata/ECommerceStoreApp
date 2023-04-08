using EventBus.Base;
using EventBus.Base.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace EventBus.RabbitMQ
{
    public class EventBusRabbitMQ : BaseEventBus
    {
        RabbitMQPersistentConnection persistentConnection;
        private readonly IConnectionFactory connectionFactory;
        private readonly IModel consumerChannel;
        private readonly ILogger logger;

        public EventBusRabbitMQ(EventBusConfig config, IServiceProvider serviceProvider) : base(serviceProvider, config)
        {
            logger = serviceProvider.GetService(typeof(ILogger<EventBusRabbitMQ>)) as ILogger<EventBusRabbitMQ>;

            if (EventBusConfig.Connection != null)
            {
                if (EventBusConfig.Connection is ConnectionFactory)
                    connectionFactory = EventBusConfig.Connection as ConnectionFactory;
                else
                {
                    var connJson = JsonConvert.SerializeObject(EventBusConfig.Connection, new JsonSerializerSettings()
                    {
                        // Self referencing loop detected for property 
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                    connectionFactory = JsonConvert.DeserializeObject<ConnectionFactory>(connJson);
                }
            }
            else
                connectionFactory = new ConnectionFactory(); //Create with default values

            persistentConnection = new RabbitMQPersistentConnection(connectionFactory, config.ConnectionRetryCount, serviceProvider);

            consumerChannel = CreateConsumerChannel();

            SubsManager.OnEventRemoved += SubsManager_OnEventRemoved;
        }

        private void SubsManager_OnEventRemoved(object? sender, string eventName)
        {
            eventName = ProcessEventName(eventName);

            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }

            consumerChannel.QueueUnbind(queue: eventName,
                exchange: EventBusConfig.DefaultTopicName,
                routingKey: eventName);

            if (SubsManager.IsEmpty)
            {
                consumerChannel.Close();
            }
        }

        #region Publish - Subscribe - Unsubscribe
        public override void Publish(IntegrationEvent @event)
        {
            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }

            var policy = Polly.Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(EventBusConfig.ConnectionRetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    logger.LogError(ex, "ERROR handling message: {ExceptionMessage} - Method : {ClassName}.{MethodName}", 
                                    ex.Message, nameof(EventBusRabbitMQ), 
                                    MethodBase.GetCurrentMethod()?.Name);
                });

            var eventName = @event.GetType().Name;
            eventName = ProcessEventName(eventName);

            consumerChannel.ExchangeDeclare(exchange: EventBusConfig.DefaultTopicName, type: "direct"); // Ensure exchange exists while publishing

            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);

            policy.Execute(() =>
            {
                logger.LogInformation("Publishing event {EventName}", eventName);

                var properties = consumerChannel.CreateBasicProperties();
                properties.DeliveryMode = 2; // persistent

                // If queue doesn't exists, this would create it and then publish. But queue should be created by consumers.
                //consumerChannel.QueueDeclare(queue: GetSubName(eventName), // Ensure queue exists while publishing
                //                     durable: true,
                //                     exclusive: false,
                //                     autoDelete: false,
                //                     arguments: null);

                //consumerChannel.QueueBind(queue: GetSubName(eventName),
                //                  exchange: EventBusConfig.DefaultTopicName,
                //                  routingKey: eventName);

                consumerChannel.BasicPublish(
                    exchange: EventBusConfig.DefaultTopicName,
                    routingKey: eventName,
                    mandatory: true,
                    basicProperties: properties,
                    body: body);
            });
        }

        public override void Subscribe<T, TH>()
        {
            var eventName = typeof(T).Name;
            eventName = ProcessEventName(eventName);

            logger.LogInformation("Subscribig event {EventName}", eventName);

            if (!SubsManager.HasSubscriptionsForEvent(eventName))
            {
                if (!persistentConnection.IsConnected)
                {
                    persistentConnection.TryConnect();
                }

                consumerChannel.QueueDeclare(queue: GetSubName(eventName), // Ensure queue exists while consuming
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                consumerChannel.QueueBind(queue: GetSubName(eventName),
                                  exchange: EventBusConfig.DefaultTopicName,
                                  routingKey: eventName);
            }

            SubsManager.AddSubscription<T, TH>();
            StartBasicConsume(eventName);
        }

        public override void UnSubscribe<T, TH>()
        {
            var eventName = typeof(T).Name;
            eventName = ProcessEventName(eventName);

            logger.LogInformation("Subscribig event {EventName}", eventName);

            SubsManager.RemoveSubscription<T, TH>();
        }
        #endregion

        #region Private methods
        private IModel CreateConsumerChannel()
        {
            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }

            var channel = persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: EventBusConfig.DefaultTopicName,
                                    type: "direct");

            return channel;
        }

        private void StartBasicConsume(string eventName)
        {
            if (consumerChannel != null)
            {
                var consumer = new EventingBasicConsumer(consumerChannel);

                consumer.Received += Consumer_Received;

                consumerChannel.BasicConsume(
                    queue: GetSubName(eventName),
                    autoAck: false,
                    consumer: consumer);
            }
        }

        private async void Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            eventName = ProcessEventName(eventName);
            var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

            try
            {
                await ProcessEvent(eventName, message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ERROR handling message: {ExceptionMessage} - Method : {ClassName}.{MethodName}",
                                ex.Message, nameof(EventBusRabbitMQ),
                                MethodBase.GetCurrentMethod()?.Name);
            }

            consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }
        #endregion
    }
}
