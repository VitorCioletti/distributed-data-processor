namespace Twitter
{
    public class CollectorConfiguration
    {
        public int MaximumAwaitingMessagesSize { get; set; }

        public int CollectDelay { get; set; }

        public string BackupFolder { get; set; }

        public long BackupDelay { get; set; }
    }
}