namespace Twitter
{
    using System;
    using System.Text;
    using RabbitMQ.Client;
    using System.Collections.Generic;

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
                Port = Configuration.MessageQueue.Port,
                VirtualHost = Configuration.MessageQueue.VirtualHost,
            }.CreateConnection();

            _channel = _connection.CreateModel();
        
            Log.Write("MessageQueue", "Initialized message queue.");
        }

        public void Finalize()
        {
            _connection.Dispose();

            _connection = null;
            _channel = null;

            Log.Write("MessageQueue", "Finalized message queue.");
        }

        public bool TrySend(IEnumerable<object> messages)
        {
            if (_connection == null)
                throw new InvalidOperationException($"Connection '{nameof(_connection)}' not initialized.");

            if (_channel == null)
                throw new InvalidOperationException($"Channel '{nameof(_channel)}' not initialized.");

            foreach (var message in messages)
            {
                // var json = JsonConvert.Serialize(message);

                var bytes = Encoding.UTF8.GetBytes(message.ToString());

                _channel.BasicPublish(Configuration.MessageQueue.Exchange, Configuration.MessageQueue.RoutingKey, true, null, bytes);

                // Log.Write("MessageQueue", $"Sent message: '{json}'.");
            }

            return true;
        }
    }
}