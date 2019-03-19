namespace Twitter
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public static class Backup
    {
        public static event Func<Message, bool> TrySend;

        public static void Write(IEnumerable<Message> messages)
        {
            using (var streamWriter = new StreamWriter(Configuration.Collector.BackupFile, true))
            {
                foreach (var message in messages)
                    streamWriter.WriteLine(message);
            }
        }

        private static void Clear(int messages)
        {
            using (var streamReader = new StreamReader(Configuration.Collector.BackupFile))
            {
 
            }
        }
    }
}