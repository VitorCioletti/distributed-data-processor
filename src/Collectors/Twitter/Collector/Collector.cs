namespace Twitter
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public static class Collector
    {
        public static event Func<Message, bool> TrySend;

        public static void Collect(Message message)
        {
            if (!TrySend(message))
                Backup.Write(message);
        }
    }
}