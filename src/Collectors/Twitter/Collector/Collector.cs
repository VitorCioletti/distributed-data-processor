namespace Twitter
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public static class Collector
    {
        public static event Func<IEnumerable<Tweet>, bool> TrySend;

        private static List<Tweet> _awaitingTweets;

        public static void Initialize()
        {
            _awaitingTweets = new List<Tweet>();
            Log.Write("Collector","Initialized collector.");
        }

        public static void Finalize()
        {
            Log.Write("Collector", "Finalized collector.");
        }

        public static void Collect(Tweet tweet)
        {
            if (_awaitingTweets == null)
                throw new Exception("Collector not initialized");

            _awaitingTweets.Add(tweet);

            if (!TrySend(_awaitingTweets))
            {
                if (_awaitingTweets.Count() > Configuration.Collector.MaximumAwaitingMessagesSize)
                    StoreAwaitingTweets();
            }
        }

        private static void StoreAwaitingTweets()
        {
            Backup.Write(_awaitingTweets);
            _awaitingTweets.Clear();
        }
    }
}