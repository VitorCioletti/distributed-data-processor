namespace Twitter
{
    public class CollectorConfiguration
    {
        public int MaximumAwaitingMessagesSize { get; set; }

        public int CollectDelay { get; set; }

        public string BackupFile { get; set; }
    }
}