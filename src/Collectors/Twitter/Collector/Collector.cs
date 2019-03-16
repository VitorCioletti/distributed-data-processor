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

        private IEnumerable<Tweet> _awaitingTweets;

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
                        _awaitingTweets.Add(Twitter.GetTweets());

                        if (!TrySend(_awaitingTweets))
                        {
                            if (_awaitingTweets.Count() > Configuration.MaximumAwaitingMessagesSize)
                                StoreAwaitingTweets();

                            _awaitingTweets.Add(message);
                        }

                        Task.Delay(Configuration.CollectDelay);
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