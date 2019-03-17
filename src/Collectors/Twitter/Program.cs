namespace Twitter
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    class Program
    {
        private static MessageQueue _messageQueue;

        static void Main(string[] args)
        {
            try
            {
                Initialize();

                Twitter.StartTweetStreaming();

                Thread.Sleep(-1);
            }
            catch (Exception e)
            {
                Log.Write("Initialization", $"An error has occurred. {e.Message} {e.StackTrace}");
            }
        }

        private static void Initialize()
        {
            ConfigureCurrentDomainEvents();
            Configuration.Initialize();

            _messageQueue = new MessageQueue();

            _messageQueue.Initialize();
            Collector.Initialize();
            Backup.Initialize();
            Twitter.Initialize();

            Func<IEnumerable<object>, bool> trySend = message => _messageQueue.TrySend(message);
 
            Collector.TrySend += trySend;
            Backup.TrySend += trySend;

            Log.Write("Initialization", "Initialized program.");
        }

        private static void ConfigureCurrentDomainEvents()
        {
            var currentDomain = AppDomain.CurrentDomain;
            
            Action finalizeConnections = () =>
            {
                Collector.Finalize();
                _messageQueue.Finalize();
            };

            currentDomain.UnhandledException += (_, e) => 
            {
                var exception = (Exception)e.ExceptionObject;

                finalizeConnections();

                Log.Write("Error", $"Unhandled exception: {exception.Message} {exception.StackTrace}");
            };

            currentDomain.ProcessExit += (_, e) => 
            {
                finalizeConnections();

                Log.Write("Finalized", "Flagged to end.");
            };
        }
    }
}