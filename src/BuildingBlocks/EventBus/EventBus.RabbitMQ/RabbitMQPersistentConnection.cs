using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Reflection;

namespace EventBus.RabbitMQ
{
    public class RabbitMQPersistentConnection : IDisposable
    {
        private IConnection connection;
        private readonly IConnectionFactory connectionFactory;
        private readonly int retryCount;
        private object lock_object = new object();
        private bool _disposed;
        private readonly ILogger logger;

        public RabbitMQPersistentConnection(IConnectionFactory connectionFactory, int retryCount, IServiceProvider serviceProvider)
        {
            logger = serviceProvider.GetService(typeof(ILogger<RabbitMQPersistentConnection>)) as ILogger<RabbitMQPersistentConnection>;

            this.connectionFactory = connectionFactory;
            this.retryCount = retryCount;
        }

        public bool IsConnected => connection != null && connection.IsOpen;

        #region CreateModel
        public IModel CreateModel()
        {
            return connection.CreateModel();
        }
        #endregion
        #region TryConnect
        public bool TryConnect()
        {
            lock (lock_object)
            {
                var policy = Policy.Handle<SocketException>()
                             .Or<BrokerUnreachableException>()
                             .WaitAndRetry(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                             {
                                 logger.LogError(ex, "ERROR handling message: {ExceptionMessage} - Method : {ClassName}.{MethodName}",
                                                 ex.Message, nameof(RabbitMQPersistentConnection),
                                                 MethodBase.GetCurrentMethod()?.Name);
                             });

                policy.Execute(() =>
                {
                    connection = connectionFactory.CreateConnection();
                });
            }

            if (IsConnected)
            {
                connection.ConnectionShutdown += Connection_ConnectionShutdown;
                connection.CallbackException += Connection_CallbackException;
                connection.ConnectionBlocked += Connection_ConnectionBlocked;
                return true;
            }
            return false;
        }

        private void Connection_ConnectionBlocked(object? sender, global::RabbitMQ.Client.Events.ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;
            TryConnect();
        }

        private void Connection_CallbackException(object? sender, global::RabbitMQ.Client.Events.CallbackExceptionEventArgs e)
        {
            if (_disposed) return;
            TryConnect();
        }

        private void Connection_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            if (_disposed) return;
            //Log
            TryConnect();
        }
        #endregion
        #region Dispose
        public void Dispose()
        {
            _disposed = true;
            connection.Dispose();
        }
        #endregion
    }
}
