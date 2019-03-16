namespace Twitter
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public class Collector
    {
        public event Func<IEnumerable<Tweet>, bool> TrySend;

        private List<Tweet> _awaitingTweets;

        public void Initialize()
        {
            _awaitingTweets = new List<Tweet>();
            Log.Write("Collector","Initialized collector.");
        }

        public void Finalize()
        {
            Log.Write("Collector", "Finalized collector.");
        }

        public void Collect()
        {
            Task.Run(
                () =>
                {
                    for (;;)
                    {
                        _awaitingTweets.AddRange(Twitter.GetTweets());

                        if (!TrySend(_awaitingTweets))
                        {
                            if (_awaitingTweets.Count() > Configuration.Collector.MaximumAwaitingMessagesSize)
                                StoreAwaitingTweets();
                        }

                        Task.Delay(Configuration.Collector.CollectDelay);
                    }
                }
            ).ContinueWith(
                (e) => Log.Write("Collector", $"{e.Exception.Message} {e.Exception.StackTrace}"),
                TaskContinuationOptions.OnlyOnFaulted
            );
        }

        private void StoreAwaitingTweets()
        {
            Backup.Write(_awaitingTweets);
            _awaitingTweets.Clear();
        }
    }
}