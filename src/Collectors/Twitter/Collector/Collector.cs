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

        private static List<Message> _awaitingMessages;

        public static void Initialize()
        {
            _awaitingMessages = new List<Message>();
            Log.Write("Collector","Initialized collector.");
        }

        public static void Finalize()
        {
            Log.Write("Collector", "Finalized collector.");
        }

        public static void Collect(Message message)
        {
            if (_awaitingMessages == null)
                throw new Exception("Collector not initialized");

            _awaitingMessages.Add(message);

            var succeded = true;

            foreach (var awaitingTweet in _awaitingMessages)
            {
                if (!TrySend(awaitingTweet))
                {
                    succeded = false;
                    break;
                }
            }

            if (!succeded)
            {
                if (_awaitingMessages.Count() > Configuration.Collector.MaximumAwaitingMessagesSize)
                    StoreAwaitingTweets();
            }
            else
                _awaitingMessages.Clear();
        }

        private static void StoreAwaitingTweets()
        {
            Backup.Write(_awaitingMessages);
            _awaitingMessages.Clear();
        }
    }
}