namespace Twitter
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Tweetinvi;
    using Tweetinvi.Streaming;

    public static class Twitter
    {
        private static IFilteredStream _stream;

        public static void Initialize()
        {
            Auth.SetUserCredentials(
                Configuration.Twitter.Consumer,
                Configuration.Twitter.ConsumerSecret,
                Configuration.Twitter.AccessToken,
                Configuration.Twitter.AccessTokenSecret
            );

            _stream = Stream.CreateFilteredStream();

            Log.Write("Twitter", "Initialized Twitter.");
        }

        public static void GetTweets()
        {
            if (_stream == null)
                throw new Exception("Twitter integration not initialized");

            _stream.AddTrack(Configuration.Twitter.WordToSearch);

            _stream.MatchingTweetReceived += (_, args) => Log.Write("Twitter", $"Received tweet: '{args.Tweet}'");
            _stream.StreamStarted += (_, __) => Log.Write("Twitter", "Started stream.");
            _stream.StreamPaused += (_, __) => Log.Write("Twitter", "Paused stream.");

            _stream.StartStreamMatchingAllConditions();

            Log.Write("Twitter", "Started getting tweets.");
        }
    }
}   