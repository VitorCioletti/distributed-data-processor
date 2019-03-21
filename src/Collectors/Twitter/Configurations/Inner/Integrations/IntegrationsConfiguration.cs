namespace Twitter
{
    public class IntegrationsConfiguration
    {
        public TwitterConfiguration Twitter { get; set; }

        public MessageQueueConfiguration MessageQueue { get; set; }
    }
}