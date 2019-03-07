namespace Collector
{
    using System;
    using System.Text;
    using RabbitMQ.Client;
    using static Configuration;

    public class MessageQueue
    {
        private IConnection _connection;

        private string _routingKey;

        private string _exchange;

        public void Initialize()
        {
            var configuration = string.Empty;

            _connection = new ConnectionFactory
            { 
                HostName = Host,
                UserName = User, 
                Password = Password, 
                Port = Port,
                VirtualHost = VirtualHost,
            }.CreateConnection();
        
            Log.Write("MessageQueue", "Initialized message queue.");
        }

        public void Finalize()
        {
            _connection.Dispose();

            _connection = null;

            Log.Write("MessageQueue", "Finalized message queue.");
        }

        public void Send(string json)
        {
            if (_connection == null)
                throw new InvalidOperationException($"Connection '{_connection}' not initialized.");

            using (var channel = _connection.CreateModel())
            {
                var bytes = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(_exchange, _routingKey, true, null, bytes);
                
                Log.Write("MessageQueue", $"Sent message: '{json}'.");
            }
        }
    }
}