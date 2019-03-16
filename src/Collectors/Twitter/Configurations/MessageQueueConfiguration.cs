namespace Twitter
{
    public class MessageQueueConfiguration
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string VirtualHost { get; set; }

        public string User { get; set; }

        public string Password { get; set; }
        
        public string Exchange { get; set; }
        
        public string RoutingKey { get; set; }
    }
}