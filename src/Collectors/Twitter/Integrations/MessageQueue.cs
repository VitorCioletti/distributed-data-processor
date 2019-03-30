namespace Twitter
{
    using System;
    using System.Text;
    using RabbitMQ.Client;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using static Configuration;

    public static class MessageQueue
    {
        private static IConnection _connection;

        private static IModel _channel;

        public static void Initialize()
        {
            var configuration = string.Empty;

            _connection = new ConnectionFactory
            { 
                HostName = Integrations.MessageQueue.Host,
                UserName = Integrations.MessageQueue.User, 
                Password = Integrations.MessageQueue.Password, 
                VirtualHost = Integrations.MessageQueue.VirtualHost,
            }.CreateConnection();

            _channel = _connection.CreateModel();
        
            Log.WriteInitialized(typeof(MessageQueue));
        }

        public static void Finalize()
        {
            _connection?.Dispose();

            _connection = null;
            _channel = null;

            Log.WriteFinalized(typeof(MessageQueue));
        }

        public static bool TrySend(Message message)
        {
            if (_connection == null)
                throw new InvalidOperationException($"Connection '{nameof(_connection)}' not initialized.");

            if (_channel == null)
                throw new InvalidOperationException($"Channel '{nameof(_channel)}' not initialized.");

            try
            {
                var json = JsonConvert.SerializeObject(message);

                var bytes = Encoding.UTF8.GetBytes(json);

                _channel.BasicPublish(
                    Integrations.MessageQueue.Exchange, 
                    Integrations.MessageQueue.RoutingKey, 
                    true, 
                    null, 
                    bytes
                );

                Log.Write("Message Queue", $"Sent message: '{json}'.");

                return true;
            }
            catch (Exception e)
            {
                Log.WriteException("Message Queue", e);

                return false;
            }
        }
    }
}