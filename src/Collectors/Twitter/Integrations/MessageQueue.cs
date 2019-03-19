namespace Twitter
{
    using System;
    using System.Text;
    using RabbitMQ.Client;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class MessageQueue
    {
        private IConnection _connection;

        private IModel _channel;

        public void Initialize()
        {
            var configuration = string.Empty;

            _connection = new ConnectionFactory
            { 
                HostName = Configuration.MessageQueue.Host,
                UserName = Configuration.MessageQueue.User, 
                Password = Configuration.MessageQueue.Password, 
                VirtualHost = Configuration.MessageQueue.VirtualHost,
            }.CreateConnection();

            _channel = _connection.CreateModel();
        
            Log.Write("Message Queue", "Initialized message queue.");
        }

        public void Finalize()
        {
            _connection?.Dispose();

            _connection = null;
            _channel = null;

            Log.Write("Message Queue", "Finalized message queue.");
        }

        public bool TrySend(Message message)
        {
            if (_connection == null)
                throw new InvalidOperationException($"Connection '{nameof(_connection)}' not initialized.");

            if (_channel == null)
                throw new InvalidOperationException($"Channel '{nameof(_channel)}' not initialized.");

            var json = JsonConvert.SerializeObject(message);

            var bytes = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(
                Configuration.MessageQueue.Exchange, 
                Configuration.MessageQueue.RoutingKey, 
                true, 
                null, 
                bytes
            );

            Log.Write("Message Queue", $"Sent message: '{json}'.");

            return true;
        }

        public bool TrySend(IEnumerable<Message> messages)
        {
            foreach (var message in messages)
            {
                if (!TrySend(message))
                    return false;
            }

            return true;
        } 
    }
}