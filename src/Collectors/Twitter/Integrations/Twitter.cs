namespace Twitter
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Tweetinvi;
    using Tweetinvi.Streaming;
    using static Configuration;

    public static class Twitter
    {
        private static IFilteredStream _stream;

        public static void Initialize()
        {
            Auth.SetUserCredentials(
                Integrations.Twitter.Consumer,
                Integrations.Twitter.ConsumerSecret,
                Integrations.Twitter.AccessToken,
                Integrations.Twitter.AccessTokenSecret
            );

            RateLimit.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;

            _stream = Stream.CreateFilteredStream();

            Log.WriteInitialized(typeof(Twitter));
        }

        public static void StartTweetStreaming()
        {
            if (_stream == null)
                throw new Exception("Twitter integration not initialized");

            _stream.AddTrack(Integrations.Twitter.WordToSearch);

            _stream.MatchingTweetReceived += (_, args) => 
            {
                Log.Write("Twitter", $"Received tweet: '{args.Tweet.FullText}'");

                Collector.Collect(
                    new Tweet
                    {
                        IdCreator = args.Tweet.Id.ToString(),
                        PostedOn = args.Tweet.CreatedAt,
                        Text = args.Tweet.FullText,
                        Subject = Integrations.Twitter.WordToSearch,
                    }
                );
            };
            _stream.StreamStarted += (_, __) => Log.Write("Twitter", "Started stream.");
            _stream.StreamPaused += (_, __) => Log.Write("Twitter", "Paused stream.");

            _stream.StartStreamMatchingAllConditions();

            Log.Write("Twitter", "Started getting tweets.");
        }
    }
}   